using System.ComponentModel.DataAnnotations;

namespace apprecipes.DataTransferObject.OtherObject
{
    public class PasswordRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "La contraseña debe tener minimo 6 caracteres.")]
        public string Password { get; set; }
    }
}

