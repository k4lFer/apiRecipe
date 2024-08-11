using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using apprecipes.DataAccess.Entity;
using apprecipes.DataTransferObject.EnumObject;
using apprecipes.DataTransferObject.ObjectGeneric;

namespace apprecipes.DataTransferObject.Object
{
    public class DtoRecipe : DtoDateGeneric
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid id { get; set; }
        public Guid idCategory { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string instruction { get; set; }
        public string ingredient { get; set; }
        public DateTime preparation { get; set; }
        public DateTime cooking { get; set; }
        public DateTime estimated { get; set; }
        public Difficulty difficulty { get; set; }
        public Guid? createdBy { get; set; }
        public Guid? updatedBy { get; set; }
        
        #region childs
        public ICollection<DtoImage> images { get; set; } = new List<DtoImage>();
        public ICollection<DtoVideo> videos { get; set; } = new List<DtoVideo>();
        #endregion
    }
}
