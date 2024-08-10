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
        public string preparation { get; set; }
        public string cooking { get; set; }
        public string estimated { get; set; }
        public Difficulty difficulty { get; set; }
        public Guid? createBy { get; set; }
        public Guid? updatedBy { get; set; }

        #region parents
        public Category ParentDtoCategory { get; set; } = null!;
        #endregion
        
        #region childs
        public ICollection<DtoImage> ChildDtoImages { get; set; } = new List<DtoImage>();
        public ICollection<DtoVideo> ChildDtoVideos { get; set; } = new List<DtoVideo>();
        #endregion
    }
}
