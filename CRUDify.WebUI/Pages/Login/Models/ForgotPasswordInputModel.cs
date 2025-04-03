using System.ComponentModel.DataAnnotations;

namespace CRUDify.WebUI.Pages.Login.Models
{
    public class ForgotPasswordInputModel
    {
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El correo no es válido")]
        public string Email { get; set; }
    }
}
