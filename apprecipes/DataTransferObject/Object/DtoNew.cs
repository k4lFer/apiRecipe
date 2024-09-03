using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using apprecipes.DataAccess.Generic;

namespace apprecipes.DataTransferObject.Object
{
    public class DtoNew : DateGeneric
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid id { get; set; }   
        public Guid? idRecipe { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 10, ErrorMessage = "El título debe tener entre 10 y 255 caracteres.")]
        public string title { get; set; }
        [Required]
        public string subtitle { get; set; }
        [Required]
        public string content { get; set; }
        public bool status { get; set; }
        [Required]
        [Url(ErrorMessage = "La URL proporcionada no es válida.")]
        public string url { get; set; }
        [Required]
        public DateTime deletedAt { get; set; }
    }
}
