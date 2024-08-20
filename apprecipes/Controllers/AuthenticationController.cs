using apprecipes.Config;
using apprecipes.DataAccess.Query;
using apprecipes.DataTransferObject.Object;
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
                _so.message = ValidateDto(so.data.dto, new List<string>() 
                { 
                    nameof(so.data.dto.username), 
                    nameof(so.data.dto.password)
                });

                if (_so.message.ExistsMessage())
                {
                    return _so;
                }
                
                QAuthentication qAuthentication = new QAuthentication();
                EncryptWithAes aes = new();
                if (!qAuthentication.ExistByUsername(aes.Encrypt(so.data.dto.username)))
                {
                    _so.message.listMessage.Add("Este usuario no se encuentra registrado en el sistema.");
                    _so.message.Error();
                    return _so;
                }
                
                _so.data.dto = qAuthentication.GetByUsername(aes.Encrypt(so.data.dto.username));
                
                if (!_so.data.dto.status)
                {
                    _so.data.dto = null;
                    _so.message.listMessage.Add("Este usuario está deshabilitado.");
                    _so.message.Warning();
                    return _so;
                }
                
                DtoUser user = qAuthentication.GetUserByIdUsername(aes.Encrypt(so.data.dto.username));
                if (BCrypt.Net.BCrypt.Verify( so.data.dto.password, _so.data.dto.password) && user != null)
                {
                    user.authetication.username = aes.Decrypt(user.authetication.username);
                    _so.data.additional  = new Tokens {
                        accessToken = await TokenUtils.GenerateAccessToken(user),
                        refreshToken = await TokenUtils.GenerateRefreshToken(user)
                    };
                    _so.data.dto.username = aes.Decrypt(_so.data.dto.username);
                    _so.data.dto.password = null;
                    _so.message.listMessage.Add("Bienvenido al sistema.");
                    _so.message.Success();
                }
                else
                {
                    _so.data.dto = null;
                    _so.message.listMessage.Add("Usuario o contraseña incorrecta, verifique.");
                    _so.message.Error();
                }

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
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<SoAuthentication>> RefreshToken([FromBody] Tokens so)
        {
            try
            {
                if (!string.IsNullOrEmpty(so.refreshToken))
                {
                    var (tokenResponse, messages) = await TokenUtils.GenerateAccessTokenFromRefreshToken(
                        so.refreshToken, AppSettings.GetRefreshJwtSecret());
                    _so.data.additional = tokenResponse;
                    _so.message = messages;
                    return _so;
                }
                _so.data = null;
                _so.message.listMessage.Add("Proporcione el (Refresh Token).");
                _so.message.Error();
                return _so;
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
