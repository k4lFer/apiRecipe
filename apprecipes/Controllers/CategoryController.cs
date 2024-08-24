using apprecipes.DataAccess.Query;
using apprecipes.DataTransferObject.OtherObject;
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
                _so.data.additional = new Pagination();
                (_so.data.listDto, _so.data.additional) = await qCategory.GetWithOffsetPagination(pageNumber, pageSize);
                if (_so.data.listDto.Count < 2)
                {
                    _so.data.dto = _so.data.listDto.First();
                    _so.data.listDto = null;
                }
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
        public ActionResult<SoCategory> GetAll()
        {
            try
            {
                QCategory qCategory = new();
                _so.data.dto = null;
                _so.data.listDto = qCategory.GetAll();
                _so.message.Success();
            }
            catch (Exception e)
            {
                _so.data.dto = null;
                _so.message.listMessage.Add("Ocurrió un error inesperado. Estamos trabajando para resolverlo.");
                _so.message.listMessage.Add("ERROR_EXCEPTION:" + e.Message);
                _so.message.Error();
            }
            return _so;
        }
    }
}
