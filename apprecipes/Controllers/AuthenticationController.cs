using apprecipes.Config;
using apprecipes.DataAccess.Query;
using apprecipes.DataTransferObject.OtherObject;
using apprecipes.Generic;
using apprecipes.Helper;
using apprecipes.ServerObjet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apprecipes.Controllers
{
    public class AuthenticationController : ControllerGeneric<SoAuthentication>
    {
        [AllowAnonymous]
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<SoAuthentication>>  Login([FromBody] SoAuthentication so)
        {
            try
            {
                _so.mo = ValidateDto(so.authetication, new List<string>() 
                { 
                    nameof(so.authetication.username), 
                    nameof(so.authetication.password)
                });

                if (_so.mo.ExistsMessage())
                {
                    return _so;
                }
                
                QAuthentication qAuthentication = new QAuthentication();

                if (!qAuthentication.ExistByUsername(so.authetication.username))
                {
                    _so.mo.listMessage.Add("Este usuario no se encuentra registrado en el sistema.");
                    _so.mo.Error();
                    return _so;
                }
                
                _so.authetication = qAuthentication.GetByUsername(so.authetication.username);
                
                if (!_so.authetication.status)
                {
                    _so.authetication = null;
                    _so.mo.listMessage.Add("Este usuario está deshabilitado.");
                    _so.mo.Warning();
                    return _so;
                }
                
                if (_so.authetication.password == so.authetication.password)
                {
                    _so.authetication.password = null;
                    _so.tokens.accessToken = await TokenUtils.GenerateAccessToken(_so.authetication);
                    _so.tokens.refreshToken = await TokenUtils.GenerateRefreshToken(_so.authetication);
                    _so.mo.listMessage.Add("Bienvenido al sistema.");
                    _so.mo.Success();
                }
                else
                {
                    _so.authetication = null;
                    _so.mo.listMessage.Add("Usuario o contraseña incorrecta, verifique.");
                    _so.mo.Error();
                }

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
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<SoAuthentication>> RefreshToken([FromBody] Tokens so)
        {
            try
            {
                if (so.refreshToken != null)
                {
                    var (tokenResponse, messages) = await TokenUtils.GenerateAccessTokenFromRefreshToken(
                        so.refreshToken, AppSettings.GetRefreshJwtSecret());
                    _so.tokens = tokenResponse;
                    _so.mo = messages;
                    return _so;
                }
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
