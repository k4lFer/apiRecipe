using apprecipes.DataAccess.Connection;
using apprecipes.DataAccess.Entity;
using apprecipes.DataTransferObject.Object;
using Microsoft.EntityFrameworkCore;

namespace apprecipes.DataAccess.Query
{
    public class QUser
    {
        public int Register(DtoUser dto)
        {
            using DataBaseContext dbc = new();
            User? user = AutoMapper.mapper.Map<User>(dto);
            dbc.Users.Add(user);
            return dbc.SaveChanges();
        }

        public bool ExistByUsername(string username)
        {
            using DataBaseContext dbc = new();
            return dbc.Users
                .Any(u => u.ChildAthentication.username == username);
        }
        
        public DtoUser MyProfile(Guid id)
        {
            using DataBaseContext dbc = new();
            var user = dbc.Users
                .Include(u => u.ChildAthentication)
                .FirstOrDefault(u => u.idAuthentication == dbc.Authentications.Find(id).id);
            return AutoMapper.mapper.Map<DtoUser>(user);
        }
        
        public DtoUser GetById(Guid id)
        {
            using DataBaseContext dbc = new();
            User? user = dbc.Users.Include(u => u.ChildAthentication).FirstOrDefault(u => u.id == id);
            return AutoMapper.mapper.Map<DtoUser>(user);
        }
        
        public DtoUser GetByUsername(string username)
        {
            using DataBaseContext dbc = new();
            User? user = dbc.Users.Include(u => u.ChildAthentication).FirstOrDefault(u => u.ChildAthentication.username == username);
            return AutoMapper.mapper.Map<DtoUser>(user);
        }
        
        public DtoUser GetByEmail(string email)
        {
            using DataBaseContext dbc = new();
            User? user = dbc.Users.Include(u => u.ChildAthentication).FirstOrDefault(u => u.email == email);
            return AutoMapper.mapper.Map<DtoUser>(user);
        }
        
        public int Update(DtoUser dto)
        {
            using DataBaseContext dbc = new();
            User? user = AutoMapper.mapper.Map<User>(dto);
            dbc.Users.Update(user);
            return dbc.SaveChanges();
        }


    }
}
