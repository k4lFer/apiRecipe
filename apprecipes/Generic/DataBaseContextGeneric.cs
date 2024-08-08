using Microsoft.EntityFrameworkCore;

namespace apprecipes.Generic
{
    public class DataBaseContextGeneric : DbContext
    {
        public DataBaseContextGeneric ()
        {
            AutoMapper.Start();
        }
    }
}
