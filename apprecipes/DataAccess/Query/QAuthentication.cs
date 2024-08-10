using apprecipes.DataAccess.Connection;
using apprecipes.DataAccess.Entity;
using apprecipes.DataTransferObject.Object;

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
    }
}
