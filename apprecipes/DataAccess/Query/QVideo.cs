using apprecipes.DataAccess.Connection;
using apprecipes.DataTransferObject.Object;

namespace apprecipes.DataAccess.Query
{
    public class QVideo
    {
        public DtoVideo GetVideoByUrl(string url)
        {
            using DataBaseContext dbc = new();
            return AutoMapper.mapper.Map<DtoVideo>(dbc.Videos.FirstOrDefault(w => w.url == url));
        }
        
        public DtoVideo GetVideoByIdRecipe(Guid idRecipe)
        {
            using DataBaseContext dbc = new();
            return AutoMapper.mapper.Map<DtoVideo>(dbc.Videos.FirstOrDefault(i => i.idRecipe == idRecipe));
        }
    }
}
