using apprecipes.DataAccess.Query;
using apprecipes.Generic;
using apprecipes.ServerObjet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apprecipes.Controllers
{
    public class ImageController : ControllerGeneric<SoImage>
    {
        [AllowAnonymous]
        [HttpGet]
        [Route("[action]")]
        public ActionResult<SoImage> GetAll()
        {
            try
            {
                _so.listDto = new QImage().GetAll();
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
