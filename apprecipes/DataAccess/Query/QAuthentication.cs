using apprecipes.DataAccess.Connection;
using apprecipes.DataTransferObject.Object;

namespace apprecipes.DataAccess.Query
{
    public class QAuthentication
    {
        public DtoAuthentication GetByUsername(string username)
        {
            using (DataBaseContext dbc = new DataBaseContext())
            {
                var x = dbc.Authentications.FirstOrDefault(u => u.username == username);
                return AutoMapper.mapper.Map<DtoAuthentication>(x);
                //return AutoMapper.mapper.Map<DtoAuthentication>(dbc.Authentications.FirstOrDefault(w => w.username == username));
            }
        }
    }
}
