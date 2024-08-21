using apprecipes.DataAccess.Connection;
using apprecipes.DataAccess.Entity;
using apprecipes.DataTransferObject.Object;
using Microsoft.EntityFrameworkCore;

namespace apprecipes.DataAccess.Query
{
    public class QLike
    {
        public async Task<int> GiveLikeAsync(DtoLike dto)
        {
            using DataBaseContext dbc = new();
            dbc.Likes.Add(AutoMapper.mapper.Map<Like>(dto));
            return await dbc.SaveChangesAsync();
        }

        public async Task<bool> HasUserLikedRecipeAsync(Guid idUser, Guid idRecipe)
        {
            using DataBaseContext dbc = new();
            return await dbc.Likes.AnyAsync(like => like.idUser == idUser && like.idRecipe == idRecipe);
        }
    }
}
