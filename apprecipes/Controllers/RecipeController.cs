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
    }
}
