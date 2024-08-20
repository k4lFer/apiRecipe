using apprecipes.DataAccess.Connection;
using apprecipes.DataAccess.Entity;
using apprecipes.DataTransferObject.Object;
using Microsoft.EntityFrameworkCore;

namespace apprecipes.DataAccess.Query
{
    public class QAuthentication
    {
        public DtoAuthentication GetByUsername(string username)
        {
            using DataBaseContext dbc = new();
            Authentication? authentication = dbc.Authentications.FirstOrDefault(u => u.username == username);
            return AutoMapper.mapper.Map<DtoAuthentication>(authentication);
        }
        
        public bool ExistByUsername(string username)
        {
            using DataBaseContext dbc = new();
            return dbc.Authentications.Any(u => u.username == username);
        }
        
        public DtoUser GetUserByIdUsername(string username)
        {
            using DataBaseContext dbc = new();
            Authentication? authentication = dbc.Authentications.FirstOrDefault(a => a.username == username);
            User? user = dbc.Users.Include(u => u.ChildAthentication)
                .FirstOrDefault(u => u.idAuthentication == authentication.id);
            return AutoMapper.mapper.Map<DtoUser>(user);
        }
    }
}
