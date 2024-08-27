using System.ComponentModel.DataAnnotations;
using apprecipes.DataTransferObject.ObjectEnum;

namespace apprecipes.DataTransferObject.Object
{
    public class DtoAuthentication
    {
        public Guid id { get; set; }
        [Required]
        [StringLength(10, MinimumLength = 4, ErrorMessage = "El nombre de usuario debe tener entre 3 y 10 caracteres.")]
        public string username { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "La contraseña debe tener minimo 6 caracteres.")]
        public string password { get; set; }
        public Role role { get; set; }
        public bool status { get; set; }
    }
}
