using apprecipes.DataAccess.Connection;
using apprecipes.DataAccess.Entity;
using apprecipes.DataTransferObject.Object;

namespace apprecipes.DataAccess.Query
{
    public class QNew
    {
        public int CreateNew(DtoNew dto)
        {
            using DataBaseContext dbc = new();
            dto.id = Guid.NewGuid();
            dto.status = true;
            dto.createdAt = DateTime.UtcNow;
            dto.updatedAt = DateTime.UtcNow;
            New news = AutoMapper.mapper.Map<New>(dto);
            dbc.News.Add(news);
            return dbc.SaveChanges();
        }
        
        public int UpdateNew(DtoNew dto)
        {
            using DataBaseContext dbc = new();
            New? news = dbc.News.FirstOrDefault(r => r.id == dto.id);
            news.idRecipe= dto.idRecipe;
            news.title = dto.title;
            news.subtitle = dto.subtitle;
            news.content = dto.content;
            news.status = dto.status;
            news.url = dto.url;
            news.deletedAt = dto.deletedAt;
            news.updatedAt = DateTime.UtcNow;
            return dbc.SaveChanges();
        }
        
        public DtoNew GetById(Guid id)
        {
            using DataBaseContext dbc = new();
            New? x = dbc.News.FirstOrDefault(w => w.id == id);
            return AutoMapper.mapper.Map<DtoNew>(x);
        }
        
        public List<DtoNew> GetAll()
        {
            using DataBaseContext dbc = new();
            return AutoMapper.mapper.Map<List<DtoNew>>(dbc.News.OrderBy(ob => ob.updatedAt).ToList());
        }
        
        public int Delete(Guid id)
        {
            using DataBaseContext dbc = new();
            New? news = dbc.News.FirstOrDefault(r => r.id == id);
            if (news != null)
            {
                dbc.News.Remove(news);
            }
            return dbc.SaveChanges();
        }
        
        public bool ExistById(Guid? id)
        {
            using DataBaseContext dbc = new();
            return dbc.News.Any(w => w.id == id);
        }
        
        public bool ExistByTitle(string title)
        {
            using DataBaseContext dbc = new();
            return dbc.News.Any(w => w.title.Replace(" ", string.Empty).ToLower().Equals(title.Replace(" ", string.Empty).ToLower()));
        }
        
        public bool ExistByUrl(string url)
        {
            using DataBaseContext dbc = new();
            return dbc.News.Any(w => w.url == url);
        }
        
        public DtoNew GetByTitle(string title)
        {
            using DataBaseContext dbc = new();
            New? x = dbc.News
                .FirstOrDefault(w => w.title.Replace(" ", string.Empty).ToLower().Equals(title.Replace(" ", string.Empty).ToLower()));
            return AutoMapper.mapper.Map<DtoNew>(x);
        }
        
        public DtoNew GetByUrl(string url)
        {
            using DataBaseContext dbc = new();
            New? x = dbc.News.FirstOrDefault(w => w.url == url);
            return AutoMapper.mapper.Map<DtoNew>(x);
        }
    }
}
