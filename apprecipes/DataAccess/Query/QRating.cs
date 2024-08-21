using apprecipes.DataAccess.Connection;
using apprecipes.DataAccess.Entity;
using apprecipes.DataTransferObject.Object;
using Microsoft.EntityFrameworkCore;

namespace apprecipes.DataAccess.Query
{
    public class QRating
    {
        public async Task<bool> IsThereRecipeRatingAsync(Guid idRecipe)
        {
            using DataBaseContext dbc = new();
            return await dbc.Ratings.AnyAsync(u => u.idRecipe == idRecipe);
        }

        public async Task<int> CreateRatingAsync(Guid idRecipe)
        {
            using DataBaseContext dbc = new();
            DtoRating dto = new()
            {
                id = Guid.NewGuid(),
                idRecipe = idRecipe,
                numberLike = 1,
                createdAt = DateTime.UtcNow,
                updatedAt = DateTime.UtcNow
            };
            dbc.Ratings.Add(AutoMapper.mapper.Map<Rating>(dto));
    
            return await dbc.SaveChangesAsync();
        }

        public async Task<int> UpdateRatingAsync(Guid idRecipe)
        {
            using DataBaseContext dbc = new();
            Rating? rating = await dbc.Ratings.FirstOrDefaultAsync(r => r.idRecipe == idRecipe);
            if (rating != null)
            {
                rating.numberLike += 1;
                rating.updatedAt = DateTime.UtcNow;
                return await dbc.SaveChangesAsync();
            }
            return 0;
        }
    }
}
