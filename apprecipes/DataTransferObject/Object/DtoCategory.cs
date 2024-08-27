using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apprecipes.DataTransferObject.Object
{
    public class DtoCategory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "El nombre debe tener entre 4 a más caracteres.")]
        public string name { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "La descripción debe tener entre 10 a más caracteres.")]
        public string description { get; set; }
    }
}
