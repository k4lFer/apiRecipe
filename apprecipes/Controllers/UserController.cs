using apprecipes.Config;
using apprecipes.DataAccess.Query;
using apprecipes.DataTransferObject.Object;
using apprecipes.DataTransferObject.ObjectEnum;
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
                _so.message = ValidateDto(so.data.dto, new List<string>() 
                {
                    nameof(so.data.dto.firstName),
                    nameof(so.data.dto.lastName),
                    nameof(so.data.dto.email),
                    nameof(so.data.dto.authetication.username),
                    nameof(so.data.dto.authetication.password)
                });
                
                QUser qUser = new();
                EncryptWithAes aes = new();
                DtoUser? userUsername = qUser.GetByUsername(aes.Encrypt(so.data.dto.authetication.username));
                if (userUsername != null && aes.Decrypt(userUsername.authetication.username) == so.data.dto.authetication.username)
                {
                    _so.message.listMessage.Add($"Este usuario ({so.data.dto.authetication.username}) ya se encuentra registrado en el sistema.");
                    _so.message.Warning();
                    return _so;
                }
                DtoUser? userEmail = qUser.GetByEmail(aes.Encrypt(so.data.dto.email));
                if (userEmail != null && aes.Decrypt(userEmail.email) == so.data.dto.email)
                {
                    _so.message.listMessage.Add($"Este correo ({so.data.dto.email}) ya se encuentra registrado en el sistema.");
                    _so.message.Warning();
                    return _so;
                }
                
                if (so.data.dto != null)
                {
                    so.data.dto.authetication.id = Guid.NewGuid();
                    so.data.dto.authetication.username = aes.Encrypt(so.data.dto.authetication.username);
                    so.data.dto.authetication.password = BCrypt.Net.BCrypt.HashPassword(so.data.dto.authetication.password);
                    so.data.dto.authetication.role = Role.Logged;
                    so.data.dto.authetication.status = true;
                    so.data.dto.id = Guid.NewGuid();
                    so.data.dto.idAuthentication = so.data.dto.authetication.id;
                    so.data.dto.email = aes.Encrypt(so.data.dto.email);
                    so.data.dto.createdAt = DateTime.UtcNow;
                    so.data.dto.updatedAt = DateTime.UtcNow;
            
                    qUser.Register(so.data.dto);
                    _so.message.listMessage.Add("Registro exitoso !.");
                    _so.message.Success();
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
                _so.data.dto = qUser.MyProfile(idAuthentication);
                if (_so.data.dto != null)
                {
                    EncryptWithAes aes = new();
                    _so.data.dto.email = aes.Decrypt(_so.data.dto.email);
                    _so.data.dto.authetication.username = aes.Decrypt(_so.data.dto.authetication.username);
                    _so.data.dto.authetication.password = null;
                    _so.message.Success();
                }
            }
            catch (Exception e)
            {
                _so.message.listMessage.Add("Ocurrió un error inesperado. Estamos trabajando para resolverlo.");
                _so.message.listMessage.Add("ERROR_EXCEPTION:" + e.Message);
                _so.message.Error();
            }
            return _so;
        }
        
        [Authorize(Roles="Admin,Other,Logged")]
        [HttpPut]
        [Route("[action]")]
        public ActionResult<SoUser> Update([FromBody] SoUser so)
        {
            try
            {
                string accessToken = Request.Headers["Authorization"].ToString();
                string userIdString = TokenUtils.GetUserIdFromAccessToken(accessToken);
                Guid idUser = new Guid(userIdString);
                
                _so.message = ValidateDto(so.data.dto, new List<string>() 
                {
                    nameof(so.data.dto.firstName),
                    nameof(so.data.dto.lastName),
                    nameof(so.data.dto.email),
                    nameof(so.data.dto.authetication.username)
                });
                
                QUser qUser = new();
                EncryptWithAes aes = new();
                DtoUser? userUsername = qUser.GetByUsername(aes.Encrypt(so.data.dto.authetication.username));
                if (userUsername != null && 
                    aes.Decrypt(userUsername.authetication.username) == so.data.dto.authetication.username
                    && idUser != userUsername.id)
                {
                    _so.message.listMessage.Add($"Este usuario ({so.data.dto.authetication.username}) ya se encuentra registrado en el sistema.");
                    _so.message.Warning();
                    return _so;
                }
                DtoUser? userEmail = qUser.GetByEmail(aes.Encrypt(so.data.dto.email));
                if (userEmail != null && aes.Decrypt(userEmail.email) == so.data.dto.email && idUser != userEmail.id)
                {
                    _so.message.listMessage.Add($"Este correo ({so.data.dto.email}) ya se encuentra registrado en el sistema.");
                    _so.message.Warning();
                    return _so;
                }
                
                DtoUser user = qUser.GetById(idUser);
                if (so.data.dto != null)
                {
                    user.firstName = so.data.dto.firstName;
                    user.lastName = so.data.dto.lastName;
                    user.email = aes.Encrypt(so.data.dto.email);
                    user.authetication.username = aes.Encrypt(so.data.dto.authetication.username);
                    user.updatedAt = DateTime.UtcNow;
            
                    qUser.Update(user);
                    _so.message.Success();
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
    }
}
