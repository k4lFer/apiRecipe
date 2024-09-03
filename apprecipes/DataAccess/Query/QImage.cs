using apprecipes.DataAccess.Connection;
using apprecipes.DataTransferObject.Object;

namespace apprecipes.DataAccess.Query
{
    public class QImage
    {
        public DtoImage GetImageByUrl(string url)
        {
            using DataBaseContext dbc = new();
            return AutoMapper.mapper.Map<DtoImage>(dbc.Images.FirstOrDefault(w => w.url == url));
        }
        
        public DtoImage GetImageByIdRecipe(Guid idRecipe)
        {
            using DataBaseContext dbc = new();
            return AutoMapper.mapper.Map<DtoImage>(dbc.Images.FirstOrDefault(i => i.idRecipe == idRecipe));
        }

        public List<DtoImage> GetAll()
        {
            using (DataBaseContext dbc = new DataBaseContext())
            {
                return AutoMapper.mapper.Map<List<DtoImage>>(dbc.Images.OrderBy(ob => ob.updatedAt).ToList());
            }
        }
        
        public bool ExistImageById(Guid id)
        {
            using DataBaseContext dbc = new();
            return dbc.Images.Any(w => w.id == id);
        }
    }
}
