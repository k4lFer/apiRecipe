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
        private static int _requestCount = 0;
        private static Guid _lastIdRecipe = Guid.Empty;
        
        [Authorize(Roles="Logged")]
        [HttpPatch]
        [Route("[action]")]
        public async Task<ActionResult<DtoMessage>> Liked( [FromBody]DtoLikeds so)
        {
            Interlocked.Increment(ref _requestCount);
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
                
                if (_lastIdRecipe != so.idRecipe)
                {
                    _requestCount = 1; 
                    _lastIdRecipe = so.idRecipe;
                }
                
                QRating qRating = new();
                QLike qLike = new();
                if (await qLike.HasUserLikedRecipeAsync(idUser, so.idRecipe))
                {
                    if (await qRating.IsThereRecipeRatingAsync(so.idRecipe))
                    {
                        if (_requestCount == 1)
                        {
                            await qRating.UpdateRatingLikedAsync(so.idRecipe);
                            await qLike.UpdateStatusAsync(idUser, so.idRecipe, true);
                            _so.message.listMessage.Add("Enhorabuena!");
                        }
                        if (_requestCount == 2)
                        {
                            await qRating.UpdateRatingDislikedAsync(so.idRecipe);
                            await qLike.UpdateStatusAsync(idUser, so.idRecipe, false);
                            _so.message.listMessage.Add("Enhoramala!");
                            _requestCount = 0;
                        }
                    }
                    else
                    {
                        await qRating.CreateRatingAsync(so.idRecipe);
                        await qLike.UpdateStatusAsync(idUser, so.idRecipe, true);
                        _so.message.listMessage.Add("Enhorabuena!");
                    }
                }
                else
                {
                    DtoLike newLike = new() { idRecipe = so.idRecipe, idUser = idUser, status = true};
                    await qLike.GiveLikeAsync(newLike);
                    
                    if (await qRating.IsThereRecipeRatingAsync(so.idRecipe))
                    {
                        if (_requestCount == 1)
                        {
                            await qRating.UpdateRatingLikedAsync(so.idRecipe);
                            _so.message.listMessage.Add("Enhorabuena!");
                        }
                    }
                    else
                    {
                        await qRating.CreateRatingAsync(so.idRecipe);
                        _so.message.listMessage.Add("Enhorabuena!");
                    }
                }

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
