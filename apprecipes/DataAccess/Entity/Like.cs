using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apprecipes.DataAccess.Entity
{
    public class Like
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid idRecipe { get; set; }       
        public Guid idUser { get; set; }       
        public bool status { get; set; }
        #region parents
        public Recipe ParentRecipe { get; set; }
        public User ParentUser { get; set; }
        #endregion
    }
}
