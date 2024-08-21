using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using apprecipes.DataAccess.Generic;
using apprecipes.DataTransferObject.EnumObject;

namespace apprecipes.DataAccess.Entity
{
    public class Recipe : DateGeneric
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
        public Guid? createdBy { get; set; }
        public Guid? updatedBy { get; set; }
        
        #region parents
        public Category ParentCategory { get; set; } = null!;
        #endregion
        
        #region childs
        public ICollection<Image> ChildImages { get; set; } = new List<Image>();
        public ICollection<Video> ChildVideos { get; set; } = new List<Video>();
        public Rating ChildRating { get; set; }
        #endregion
    }
}
