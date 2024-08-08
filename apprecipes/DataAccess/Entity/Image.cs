using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apprecipes.DataAccess.Entity
{
    [Table("images")]
    public class Image
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid id { get; set; }
        public Guid idRecipe { get; set; }
        public string data { get; set; }
        public string extension { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updatedAt { get; set; }
    }
}
