using apprecipes.DataAccess.Connection;
using apprecipes.DataTransferObject.Object;

namespace apprecipes.DataAccess.Query
{
    public class QUser
    {
        public List<DtoUser> GetAll()
        {
            using (DataBaseContext dbc = new DataBaseContext())
            {
                return AutoMapper.mapper.Map<List<DtoUser>>(dbc.Users.OrderBy(ob => ob.email).ToList());
            }
        }   
    }
}
