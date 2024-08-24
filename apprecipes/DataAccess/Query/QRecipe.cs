using apprecipes.DataAccess.Connection;
using apprecipes.DataAccess.Entity;
using apprecipes.DataTransferObject.Object;
using apprecipes.DataTransferObject.OtherObject;
using Microsoft.EntityFrameworkCore;

namespace apprecipes.DataAccess.Query
{
    public class QRecipe
    {
        public DtoRecipe GetByTitle(string title)
        {
            using DataBaseContext dbc = new();
            Recipe? x = dbc.Recipes
                .FirstOrDefault(w => w.title.Replace(" ", string.Empty).Equals(title.Replace(" ", string.Empty)));
            return AutoMapper.mapper.Map<DtoRecipe>(x);
        }

        public bool ExistByTitle(string title)
        {
            using DataBaseContext dbc = new();
            return dbc.Recipes.Any(w => w.title.Replace(" ", string.Empty).Equals(title.Replace(" ", string.Empty)));
        }
        
        public bool ExistById(Guid id)
        {
            using DataBaseContext dbc = new();
            return dbc.Recipes.Any(w => w.id == id);
        }
        
        public DtoRecipe GetByIdIncludeImageVideo(Guid id)
        {
            using DataBaseContext dbc = new();
            Recipe? recipe = dbc.Recipes
                .Include(i => i.ChildImages)
                .Include(v => v.ChildVideos)
                .Include(r => r.ChildRating)
                .FirstOrDefault(r => r.id == id);
            return AutoMapper.mapper.Map<DtoRecipe>(recipe);
        }

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

        public int CreateRecipe(DtoRecipe dtoRecipe, Guid idUser)
        {
            using DataBaseContext dbc = new();
            dtoRecipe.id = Guid.NewGuid();
            dtoRecipe.createdAt = DateTime.UtcNow;
            dtoRecipe.updatedAt = DateTime.UtcNow;
            dtoRecipe.createdBy = idUser;
            dtoRecipe.updatedBy = idUser;
            
            foreach (DtoImage dtoImage in dtoRecipe.images)
            {
                dtoImage.id = Guid.NewGuid();
                dtoImage.idRecipe = dtoRecipe.id;
                dtoImage.createdAt = DateTime.UtcNow;
                dtoImage.updatedAt = DateTime.UtcNow;
            }

            if (dtoRecipe.videos?.Count > 0)
            {
                foreach (DtoVideo dtoVideo in dtoRecipe.videos)
                {
                    dtoVideo.id = Guid.NewGuid();
                    dtoVideo.idRecipe = dtoRecipe.id;
                    dtoVideo.createdAt = DateTime.UtcNow;
                    dtoVideo.updatedAt = DateTime.UtcNow;
                }
            }
            
            Recipe recipe = AutoMapper.mapper.Map<Recipe>(dtoRecipe);
            dbc.Recipes.Add(recipe);
            return dbc.SaveChanges();
        }
        
        public int UpdateRecipe(DtoRecipe dtoRecipe, Guid idUser)
        {
            using DataBaseContext dbc = new();

            Recipe? recipe = dbc.Recipes
                .Include(i => i.ChildImages)
                .Include(v => v.ChildVideos)
                .FirstOrDefault(r => r.id == dtoRecipe.id);
            
            recipe.title = dtoRecipe.title;
            recipe.description = dtoRecipe.description;
            recipe.instruction = dtoRecipe.description;
            recipe.ingredient = dtoRecipe.ingredient;
            recipe.preparation = dtoRecipe.preparation;
            recipe.cooking = dtoRecipe.cooking;
            recipe.estimated = dtoRecipe.estimated;
            recipe.difficulty = dtoRecipe.difficulty;
            recipe.updatedAt = DateTime.UtcNow;
            recipe.updatedBy = idUser;
            
            foreach (DtoImage dtoImage in dtoRecipe.images)
            {
                Image? existingImage = recipe.ChildImages.FirstOrDefault(img => img.url == dtoImage.url);
                if (existingImage == null)
                {
                    recipe.ChildImages.Add(new Image
                    {
                        id = Guid.NewGuid(),
                        idRecipe = dtoRecipe.id,
                        url = dtoImage.url,
                        createdAt = DateTime.UtcNow,
                        updatedAt = DateTime.UtcNow
                    });
                }
            }

            foreach (var dtoVideo in dtoRecipe.videos)
            {
                Video? existingVideo = recipe.ChildVideos.FirstOrDefault(vid => vid.url == dtoVideo.url);
                if (existingVideo != null)
                {
                    existingVideo.title = dtoVideo.title;
                    existingVideo.description = dtoVideo.description;
                    existingVideo.updatedAt = DateTime.UtcNow;
                }
                else
                {
                    recipe.ChildVideos.Add(new Video
                    {
                        id = Guid.NewGuid(),
                        idRecipe = dtoRecipe.id,
                        url = dtoVideo.url,
                        title = dtoVideo.title,
                        description = dtoVideo.description,
                        createdAt = DateTime.UtcNow,
                        updatedAt = DateTime.UtcNow
                    });
                }
            }
            
            return dbc.SaveChanges();
        }
    }
}
