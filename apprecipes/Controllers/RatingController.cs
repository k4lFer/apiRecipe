using apprecipes.Config;
using apprecipes.DataAccess.Query;
using apprecipes.DataTransferObject.Object;
using apprecipes.Generic;
using apprecipes.ServerObjet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apprecipes.Controllers
{
    public class RatingController : ControllerGeneric<SoRating>
    {
        [Authorize(Roles="Logged")]
        [HttpPatch]
        [Route("[action]")]
        public async Task<ActionResult<SoRating>> Liked( [FromBody]SoRating so)
        {
            try
            {
                Guid idUser = Guid.Parse(TokenUtils.GetUserIdFromAccessToken(Request.Headers["Authorization"].ToString()));
                if (idUser == Guid.Empty)
                {
                    _so.message.listMessage.Add("Access Token no identificado.");
                    _so.message.Error();
                    return _so;
                }
                
                _so.message = ValidateDto(so.data.dto, new List<string> { nameof(so.data.dto.idRecipe) });
                if (_so.message.ExistsMessage()) return _so;
                
                if (so.data.dto.idRecipe == Guid.Empty)
                {
                    _so.message.listMessage.Add("Proporcione una id de receta valida.");
                    _so.message.Error();
                    return _so;
                }
                
                QLike qLike = new();
                if (await qLike.HasUserLikedRecipeAsync(idUser, so.data.dto.idRecipe))
                {
                    _so.message.listMessage.Add("Usted ya le dio me gusta a esta receta.");
                    _so.message.Warning();
                    return _so;
                }

                QRating qRating = new();
                if (await qRating.IsThereRecipeRatingAsync(so.data.dto.idRecipe))
                {
                    await qRating.UpdateRatingAsync(so.data.dto.idRecipe);
                }
                else
                {
                    await qRating.CreateRatingAsync(so.data.dto.idRecipe);
                }
                DtoLike newLike = new() { idRecipe = so.data.dto.idRecipe, idUser = idUser };
                await qLike.GiveLikeAsync(newLike);
                
                _so.message.Success();
            }
            catch (Exception ex)
            {
                _so.message.listMessage.Add("Ocurrió un error inesperado. Estamos trabajando para resolverlo.");
                _so.message.listMessage.Add("ERROR_EXCEPTION:" + ex.Message);
                _so.message.Error();
            }
            return _so;
        }
    }
}
