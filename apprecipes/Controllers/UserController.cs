using System.Security.Claims;
using apprecipes.Config;
using apprecipes.DataAccess.Query;
using apprecipes.DataTransferObject.EnumObject;
using apprecipes.DataTransferObject.Object;
using apprecipes.Generic;
using apprecipes.ServerObjet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apprecipes.Controllers
{
    public class UserController : ControllerGeneric<SoUser>
    {
        [AllowAnonymous]
        [HttpPost]
        [Route("[action]")]
        public ActionResult<SoUser> Register([FromBody] SoUser so)
        {
            try
            {
                _so.mo = ValidateDto(so.dto, new List<string>() 
                {
                    nameof(so.dto.firstName),
                    nameof(so.dto.lastName),
                    nameof(so.dto.email),
                    nameof(so.dto.authetication.username),
                    nameof(so.dto.authetication.password)
                });
                
                QUser qUser = new();
                EncryptWithAes aes = new();
                DtoUser? user = qUser.GetByUsername(aes.Encrypt(so.dto.authetication.username));
                if (user != null && BCrypt.Net.BCrypt.Verify(so.dto.authetication.password, user.authetication.password))
                {
                    _so.mo.listMessage.Add($"Este usuario ({so.dto.authetication.username}) ya se encuentra registrado en el sistema.");
                    _so.mo.Warning();
                    return _so;
                }
                if (user != null && aes.Decrypt(user.email) == so.dto.email)
                {
                    _so.mo.listMessage.Add($"Este correo ({so.dto.email}) ya se encuentra registrado en el sistema.");
                    _so.mo.Warning();
                    return _so;
                }
                if (so.dto != null)
                {
                    so.dto.authetication.id = Guid.NewGuid();
                    so.dto.authetication.username = aes.Encrypt(so.dto.authetication.username);
                    so.dto.authetication.password = BCrypt.Net.BCrypt.HashPassword(so.dto.authetication.password);
                    so.dto.authetication.role = Role.Logged;
                    so.dto.authetication.status = true;
                    so.dto.id = Guid.NewGuid();
                    so.dto.idAuthentication = so.dto.authetication.id;
                    so.dto.email = aes.Encrypt(so.dto.email);
                    so.dto.createdAt = DateTime.UtcNow;
                    so.dto.updatedAt = DateTime.UtcNow;
            
                    qUser.Register(so.dto);
                    _so.mo.Success();
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
        
        [Authorize(Roles="Admin,Other,Logged")]
        [HttpGet]
        [Route("[action]")]
        public ActionResult<SoUser> MyProfile()
        {
            try
            {
                string accessToken = Request.Headers["Authorization"].ToString();
                string userIdString = TokenUtils.GetUserIdFromAccessToken(accessToken);
                Guid idAuthentication = new Guid(userIdString);
                QUser qUser = new();
                _so.dto = qUser.MyProfile(idAuthentication);
                if (_so.dto != null)
                {
                    EncryptWithAes aes = new();
                    _so.dto.email = aes.Decrypt(_so.dto.email);
                    _so.dto.authetication.username = aes.Decrypt(_so.dto.authetication.username);
                    _so.dto.authetication.password = null;
                }
            }
            catch (Exception e)
            {
                _so.mo.listMessage.Add("Ocurrió un error inesperado. Estamos trabajando para resolverlo.");
                _so.mo.listMessage.Add("ERROR_EXCEPTION:" + e.Message);
                _so.mo.Error();
            }
            return _so;
        }
        
        //falta comprobar
        [Authorize(Roles="Admin,Other,Logged")]
        [HttpPut]
        [Route("[action]")]
        public ActionResult<SoUser> Update([FromBody] SoUser so)
        {
            try
            {
                _so.mo = ValidateDto(so.dto, new List<string>() 
                {
                    nameof(so.dto.id),
                    nameof(so.dto.firstName),
                    nameof(so.dto.lastName),
                    nameof(so.dto.email),
                    nameof(so.dto.authetication.username),
                    nameof(so.dto.authetication.password)
                });
                
                QUser qUser = new();
                EncryptWithAes aes = new();
                DtoUser? user = qUser.GetByUsername(aes.Encrypt(so.dto.authetication.username));
                if (user != null && BCrypt.Net.BCrypt.Verify(so.dto.authetication.password, user.authetication.password))
                {
                    _so.mo.listMessage.Add($"Este usuario ({so.dto.authetication.username}) ya existe.");
                    _so.mo.Warning();
                    return _so;
                }
                if (user != null && aes.Decrypt(user.email) == so.dto.email)
                {
                    _so.mo.listMessage.Add($"Este correo ({so.dto.email}) ya existe.");
                    _so.mo.Warning();
                    return _so;
                }
                if (so.dto != null)
                {
                    so.dto.authetication.username = aes.Encrypt(so.dto.authetication.username);
                    so.dto.authetication.password = BCrypt.Net.BCrypt.HashPassword(so.dto.authetication.password);
                    so.dto.idAuthentication = user.authetication.id;
                    so.dto.email = aes.Encrypt(so.dto.email);
                    so.dto.updatedAt = DateTime.UtcNow;
            
                    qUser.Update(so.dto);
                    _so.mo.Success();
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
