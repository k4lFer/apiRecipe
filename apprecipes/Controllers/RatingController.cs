using apprecipes.Config;
using apprecipes.DataAccess.Query;
using apprecipes.DataTransferObject.Object;
using apprecipes.DataTransferObject.OtherObject;
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
        public async Task<ActionResult<DtoMessage>> Liked( [FromBody]DtoLikeds so)
        {
            try
            {
                Guid idUser = Guid.Parse(TokenUtils.GetUserIdFromAccessToken(Request.Headers["Authorization"].ToString()));
                if (so.idRecipe == Guid.Empty)
                {
                    _so.message.listMessage.Add("Proporcione una ID de receta válida.");
                    _so.message.Error();
                    return BadRequest(_so.message);
                }

                QRecipe qRecipe = new();
                if (!qRecipe.ExistById(so.idRecipe))
                {
                    _so.message.listMessage.Add("La receta no se encuentra disponible.");
                    _so.message.Error();
                    return Conflict(_so.message);
                }
                
                QLike qLike = new();
                if (await qLike.HasUserLikedRecipeAsync(idUser, so.idRecipe))
                {
                    _so.message.listMessage.Add("Usted ya le dio me gusta a esta receta.");
                    _so.message.Warning();
                    return Unauthorized(_so.message);
                }

                QRating qRating = new();
                if (await qRating.IsThereRecipeRatingAsync(so.idRecipe))
                {
                    await qRating.UpdateRatingAsync(so.idRecipe);
                }
                else
                {
                    await qRating.CreateRatingAsync(so.idRecipe);
                }
                
                DtoLike newLike = new() { idRecipe = so.idRecipe, idUser = idUser };
                
                await qLike.GiveLikeAsync(newLike);
                _so.message.listMessage.Add("Enhorabuena!");
                _so.message.Success();
            }
            catch (Exception ex)
            {
                _so.message.listMessage.Add(ex.Message);
                _so.message.Exception();
                return StatusCode(500, _so.message);
            }
            return _so.message;
        }
    }
}
