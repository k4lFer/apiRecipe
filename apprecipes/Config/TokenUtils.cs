using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using apprecipes.DataTransferObject.EnumObject;
using apprecipes.DataTransferObject.Object;
using apprecipes.DataTransferObject.OtherObject;
using apprecipes.Helper;
using Microsoft.IdentityModel.Tokens;

namespace apprecipes.Config
{
    public class TokenUtils
    {
        public static Task<string> GenerateAccessToken(DtoAuthentication user)
        {
            SymmetricSecurityKey authSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(AppSettings.GetAccessJwtSecret())
            );
            
            List<Claim> claims =
            [
                new Claim("id", user.id.ToString()),
                new Claim(ClaimTypes.Role, user.role.ToString())
            ];
            
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = AppSettings.GetOriginIssuer(),
                Audience = AppSettings.GetOriginAudience(),
                Expires = DateTime.UtcNow.AddHours(24),
                NotBefore = DateTime.UtcNow,
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims)
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return Task.FromResult(tokenHandler.WriteToken(token));
        }

        public static Task<string>  GenerateRefreshToken(DtoAuthentication user)
        {
            SymmetricSecurityKey authSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(AppSettings.GetRefreshJwtSecret())
            );
            
            List<Claim> claims =
            [
                new Claim("id", user.id.ToString()),
                new Claim("username", user.username),
                new Claim("status", user.status.ToString()),
                new Claim(ClaimTypes.Role, user.role.ToString())
            ];
            
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = AppSettings.GetOriginIssuer(),
                Audience = AppSettings.GetOriginAudience(),
                Expires = DateTime.UtcNow.AddDays(7),
                NotBefore = DateTime.UtcNow,
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims)
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return Task.FromResult(tokenHandler.WriteToken(token));
        }
        
        public static async Task<(Tokens, DtoMessage)> GenerateAccessTokenFromRefreshToken(string refreshToken, string secret)
        {
            Tokens tokens = new Tokens();
            DtoMessage message = new DtoMessage();
            try
            {
                ClaimsPrincipal primary = await ValidateToken(refreshToken, secret);
                if (primary == null)
                {
                    message.listMessage.Add("Token de actualización no recibido.");
                    return (tokens, message);
                }

                Claim userClaim = primary.Claims.FirstOrDefault(c => c.Type == "id");
                if (userClaim == null)
                {
                    message.listMessage.Add("Token de actualización no válido.");
                    return (tokens, message);
                }

                DtoAuthentication user = new DtoAuthentication
                {
                    id = Guid.Parse(userClaim.Value),
                    username = primary.Claims.FirstOrDefault(c => c.Type == "username")?.Value,
                    role = (Role)Enum.Parse(typeof(Role), primary.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value)
                };
                
                message.Success();
                tokens.accessToken = await GenerateAccessToken(user);
                tokens.refreshToken = refreshToken;
                return (tokens, message);
            }
            catch (SecurityTokenException ex)
            {
                message.listMessage.Add(ex.Message);
                return (tokens, message);
            }
        }

        private static Task<ClaimsPrincipal> ValidateToken(string token, string secret)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(secret);
            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = AppSettings.GetOriginIssuer(),
                ValidAudience = AppSettings.GetOriginAudience(),
                ClockSkew = TimeSpan.Zero
            };
            try
            {
                ClaimsPrincipal primary = tokenHandler.ValidateToken(
                    token,
                    validationParameters,
                    out SecurityToken validatedToken
                );
                return Task.FromResult(
                    validatedToken is JwtSecurityToken jwtToken && jwtToken.Header.Alg.Equals(
                        SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase)
                        ? primary
                        : null
                );
            }
            catch
            {
                return null;
            }
        }

        public static string GetUserIdFromAccessToken(string accessToken)
        {
            String token = accessToken.Replace("Bearer ", "");
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
            Claim accessClaim = jwtToken?.Claims.FirstOrDefault(claim => claim.Type == "id");

            if (accessClaim != null)
            {
                return accessClaim.Value;
            }
            else
            {
                return null; 
            }
        }
    }
}
