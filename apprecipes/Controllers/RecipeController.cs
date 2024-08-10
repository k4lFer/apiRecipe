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
        public async Task<ActionResult<SoRecipe>> GetRecipesByCategory(Guid idCategory, int pageNumber, int pageSize)
        {
            QRecipe qRecipe = new();
            (_so.listDto, _so.pagination) = await qRecipe.GetRecipesByCategory(idCategory,pageNumber, pageSize);
            _so.mo.Success();
            return _so;
        }
    }
}
