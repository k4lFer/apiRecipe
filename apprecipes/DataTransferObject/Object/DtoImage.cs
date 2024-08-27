using System.ComponentModel.DataAnnotations;
using apprecipes.DataTransferObject.ObjectGeneric;

namespace apprecipes.DataTransferObject.Object
{
    public class DtoImage : DtoDateGeneric
    {
        public Guid id { get; set; }
        public Guid idRecipe { get; set; }
        [Required]
        [Url(ErrorMessage = "La URL proporcionada no es válida.")]

        public string url { get; set; }
    }
}
