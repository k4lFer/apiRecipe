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
    
            // Obtener el total de registros para la categoría
            int totalRecords = await dbc.Recipes
                .Where(r => r.idCategory == idCategory)
                .CountAsync();
    
            // Calcular el total de páginas
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            // Obtener las recetas con la paginación y filtros
            ICollection<Recipe> recipes = await dbc.Recipes
                .AsNoTracking()
                .Where(r => r.idCategory == idCategory)
                .OrderBy(r => r.title) // Ordenar por título
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(r => r.ChildImages)
                .Include(r => r.ChildVideos)
                .ToListAsync();

            // Mapear a DTO
            ICollection<DtoRecipe> listDtoRecipes = AutoMapper.mapper.Map<ICollection<DtoRecipe>>(recipes);

            // Crear la información de paginación
            Pagination pagination = new Pagination
            {
                pageNumber = pageNumber,
                pageSize = pageSize,
                totalPages = totalPages,
                totalRecords = totalRecords
            };

            return (listDtoRecipes, pagination);
        }
    }
}
