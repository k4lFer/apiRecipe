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
        
        public async Task<DtoLike> GetByIdsAsync(Guid idUser, Guid idRecipe)
        {
            using DataBaseContext dbc = new();
            Like? like = await dbc.Likes.FirstOrDefaultAsync(like => like.idUser == idUser && like.idRecipe == idRecipe);
            return AutoMapper.mapper.Map<DtoLike>(like);
        }
        
        public async Task<int> UpdateStatusAsync(Guid idUser, Guid idRecipe, bool status)
        {
            using DataBaseContext dbc = new();
            var rowsAffected = await dbc.Database.ExecuteSqlRawAsync(
                "UPDATE likes SET status = {0} WHERE idUser = {1} AND idRecipe = {2}",
                status, idUser, idRecipe
            );
            return rowsAffected;
            // Se usó SQL directo para asegurar la actualización
            // La consulta con el ORM no sorporta esta actualizacion, el problema es que afecta dos filas ocasionando fallos.
            /*
             *     var like = await dbc.Likes.Where(l => l.idUser == idUser && l.idRecipe == idRecipe).SingleOrDefaultAsync();
                   if (like != null)
                   {
                       like.status = status;
                       return await dbc.SaveChangesAsync();
                   }
             */
        }
    }
}
