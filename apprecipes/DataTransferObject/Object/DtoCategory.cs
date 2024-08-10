using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apprecipes.DataTransferObject.Object
{
    public class DtoCategory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid id { get; set; }
        public string name { get; set; }
        public string? description { get; set; }
        
        #region childs
        public ICollection<DtoRecipe> ChildDtoRecipes { get; set; } = new List<DtoRecipe>();
        #endregion
    }
}
