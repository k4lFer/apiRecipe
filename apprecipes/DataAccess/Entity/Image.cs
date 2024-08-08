using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apprecipes.Models
{
    [Table("images")]
    public class Image
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string id { get; set; }
        public string idRecipe { get; set; }
        public string data { get; set; }
        public string extension { get; set; }
        public DateTime createAt { get; set; }
        public DateTime updatedAt { get; set; }
    }
}
