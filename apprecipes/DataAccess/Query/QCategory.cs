using apprecipes.DataAccess.Connection;
using apprecipes.DataAccess.Entity;
using apprecipes.DataTransferObject.Object;
using apprecipes.DataTransferObject.OtherObject;
using Microsoft.EntityFrameworkCore;

namespace apprecipes.DataAccess.Query
{
    public class QCategory
    {   
        public async Task<(ICollection<DtoCategory>, Pagination)> GetWithOffsetPagination(int pageNumber, int pageSize)
        {
            await using DataBaseContext dbc = new();
            int totalRecords = await dbc.Categories.CountAsync();
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            ICollection<Category> categories = await dbc.Categories
                .AsNoTracking()
                .OrderBy(p => p.name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            ICollection<DtoCategory> listDtoCategories = AutoMapper.mapper.Map<ICollection<DtoCategory>>(categories);
            Pagination pagination = new Pagination
            {
                pageNumber = pageNumber,
                pageSize = pageSize,
                totalPages = totalPages,
                totalRecords = totalRecords
            };
            return (listDtoCategories, pagination);
        }
        
        public List<DtoCategory> GetAll()
        {
            using DataBaseContext dbc = new();
            return AutoMapper.mapper.Map<List<DtoCategory>>(dbc.Categories.OrderBy(ob => ob.name).ToList());
        }
        
        public int Create(DtoCategory dto)
        {
            using DataBaseContext dbc = new();
            dto.id = Guid.NewGuid();
            Category? category = AutoMapper.mapper.Map<Category>(dto);
            dbc.Categories.Add(category);
            return dbc.SaveChanges();
        }
        
        public int Update(DtoCategory dto)
        {
            using DataBaseContext dbc = new();
            Category? category = AutoMapper.mapper.Map<Category>(dto);
            dbc.Categories.Update(category);
            return dbc.SaveChanges();
        }
        
        public DtoCategory GetByName(string name)
        {
            using var dbc = new DataBaseContext();
            var category = dbc.Categories
                .FirstOrDefault(w => w.name.Replace(" ", string.Empty) == name.Replace(" ", string.Empty));
            return AutoMapper.mapper.Map<DtoCategory>(category);
        }
        
        public DtoCategory GetById(Guid id)
        {
            using DataBaseContext dbc = new();
            Category? user = dbc.Categories.FirstOrDefault(u => u.id == id);
            return AutoMapper.mapper.Map<DtoCategory>(user);
        }
        
        public bool ExistCategoryById(Guid id)
        {
            using DataBaseContext dbc = new();
            return dbc.Categories.Any(w => w.id == id);
        }
        
        public int Delete(Guid id)
        {
            using DataBaseContext dbc = new();
            Category? category = dbc.Categories.FirstOrDefault(i => i.id == id);
            if (category != null)
            {
                dbc.Categories.Remove(category);
            }
            return dbc.SaveChanges();
        }
    }
}
