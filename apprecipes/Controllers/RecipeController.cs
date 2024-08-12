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
                (_so.listDto, _so.pagination) = await qRecipe.GetRecipesByCategory(idCategory,pageNumber, pageSize);
                _so.mo.Success();
            }
            catch (Exception ex)
            {
                _so.mo.listMessage.Add("Ocurrió un error inesperado. Estamos trabajando para resolverlo.");
                _so.mo.listMessage.Add("ERROR_EXCEPTION:" + ex.Message);
                _so.mo.Error();
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
                _so.dto = qRecipe.TheMostLiked();
                _so.mo.Success();
            }
            catch (Exception ex)
            {
                _so.mo.listMessage.Add("Ocurrió un error inesperado. Estamos trabajando para resolverlo.");
                _so.mo.listMessage.Add("ERROR_EXCEPTION:" + ex.Message);
                _so.mo.Error();
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
                _so.mo = ValidateDto(so.dto, new List<string>() 
                {
                    nameof(so.dto.title),
                    nameof(so.dto.description),
                    nameof(so.dto.instruction),
                    nameof(so.dto.preparation),
                    nameof(so.dto.cooking),
                    nameof(so.dto.estimated),
                    nameof(so.dto.difficulty),
                    nameof(so.dto.images),
                    nameof(so.dto.videos),
                });
                QRecipe qRecipe = new();
                
                //falta
            }
            catch (Exception ex)
            {
                _so.mo.listMessage.Add("Ocurrió un error inesperado. Estamos trabajando para resolverlo.");
                _so.mo.listMessage.Add("ERROR_EXCEPTION:" + ex.Message);
                _so.mo.Error();
            }
            return _so;
        }
    }
}
