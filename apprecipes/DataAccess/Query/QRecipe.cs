using apprecipes.DataAccess.Connection;
using apprecipes.DataAccess.Entity;
using apprecipes.DataTransferObject.Object;
using apprecipes.DataTransferObject.OtherObject;
using Microsoft.EntityFrameworkCore;

namespace apprecipes.DataAccess.Query
{
    public class QRecipe
    {
        public async Task<(ICollection<DtoRecipe>, Pagination)> GetRecipesByCategory(Guid idCategory, int pageNumber, int pageSize)
        {
            await using DataBaseContext dbc = new();
            int totalRecords = await dbc.Recipes
                .Where(r => r.idCategory == idCategory)
                .CountAsync();
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            
            ICollection<Recipe> recipes = await dbc.Recipes
                .AsNoTracking()
                .Where(r => r.idCategory == idCategory)
                .OrderBy(r => r.title)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(r => r.ChildImages)
                .Include(r => r.ChildVideos)
                .ToListAsync();

            ICollection<DtoRecipe> listDtoRecipes = AutoMapper.mapper.Map<ICollection<DtoRecipe>>(recipes);
            Pagination pagination = new Pagination
            {
                pageNumber = pageNumber,
                pageSize = pageSize,
                totalPages = totalPages,
                totalRecords = totalRecords
            };
            return (listDtoRecipes, pagination);
        }
        
        public DtoRecipe TheMostLiked()
        {
            using DataBaseContext dbc = new();
            Rating? mostLikedRating = dbc.Ratings
                .OrderByDescending(rt => rt.numberLike)
                .FirstOrDefault();
            Recipe? recipe = dbc.Recipes
                .Include(r => r.ChildImages)
                .Include(r => r.ChildVideos)
                .Include(r=>r.ChildRating)
                .FirstOrDefault(r => mostLikedRating != null && r.id == mostLikedRating.idRecipe);
            return AutoMapper.mapper.Map<DtoRecipe>(recipe);
        } 
    }
}
