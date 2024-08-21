using apprecipes.Config;
using apprecipes.DataAccess.Query;
using apprecipes.Generic;
using apprecipes.ServerObjet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apprecipes.Controllers
{
    public class RecipeController : ControllerGeneric<SoRecipe>
    {
        [AllowAnonymous]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<SoRecipe>> PaginationByCategory(Guid idCategory, int pageNumber, int pageSize)
        {
            try
            {
                QRecipe qRecipe = new();
                (_so.data.listDto, _so.data.additional) = await qRecipe.GetRecipesByCategory(idCategory,pageNumber, pageSize);
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
        
        [AllowAnonymous]
        [HttpGet]
        [Route("[action]")]
        public ActionResult<SoRecipe> TheMostLiked()
        {
            try
            {
                QRecipe qRecipe = new();
                _so.data.dto = qRecipe.TheMostLiked();
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

        [Authorize(Roles="Admin")]
        [HttpPost]
        [Route("[action]")]
        public ActionResult<SoRecipe> Create([FromBody] SoRecipe so)
        {
            try
            {
                _so.message = ValidateDto(so.data.dto, new List<string>() 
                {
                    nameof(so.data.dto.title),
                    nameof(so.data.dto.description),
                    nameof(so.data.dto.instruction),
                    nameof(so.data.dto.preparation),
                    nameof(so.data.dto.cooking),
                    nameof(so.data.dto.estimated),
                    nameof(so.data.dto.difficulty),
                    nameof(so.data.dto.images),
                    nameof(so.data.dto.videos),
                });
                
                if (_so.message.ExistsMessage())
                {
                    return _so;
                }
                QRecipe qRecipe = new();
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
