using apprecipes.DataAccess.Query;
using apprecipes.Generic;
using apprecipes.ServerObjet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apprecipes.Controllers
{
    public class CategoryController : ControllerGeneric<SoCategory>
    {
        [AllowAnonymous]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<SoCategory>> GetByPagination(int pageNumber, int pageSize)
        {
            try
            {
                QCategory qCategory = new();
                (_so.listDto, _so.pagination) = await qCategory.GetWithOffsetPagination(pageNumber, pageSize);
                if (_so.listDto.Count < 2)
                {
                    _so.dto = _so.listDto.First();
                    _so.listDto = null;
                }
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
