using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apprecipes.DataAccess.Entity
{
    public class Category
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid id { get; set; }
        public string name { get; set; }
        public string? description { get; set; }
        
        #region childs
        public ICollection<Recipe> ChildRecipes { get; set; } = new List<Recipe>();
        #endregion
    }
}
