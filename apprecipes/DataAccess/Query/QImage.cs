using apprecipes.DataAccess.Connection;
using apprecipes.DataTransferObject;

namespace apprecipes.DataAccess.Query
{
    public class QImage
    {
        public List<DtoImage> GetAll()
        {
            using (DataBaseContext dbc = new DataBaseContext())
            {
                return AutoMapper.mapper.Map<List<DtoImage>>(dbc.Images.OrderBy(ob => ob.updatedAt).ToList());
            }
        }
    }
}
