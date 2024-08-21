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
    }
}
