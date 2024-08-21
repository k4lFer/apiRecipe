using apprecipes.DataAccess.Generic;

namespace apprecipes.DataAccess.Entity
{
    public class Rating : DateGeneric
    {
        public Guid id { get; set; }
        public Guid idRecipe { get; set; }
        public long numberLike { get; set; }
        
        #region parents
        public Recipe ParentRecipe { get; set; } = null!;
        #endregion
    }
}
