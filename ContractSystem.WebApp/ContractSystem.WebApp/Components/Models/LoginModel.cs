using System.ComponentModel.DataAnnotations;

namespace ContractSystem.WebApp.Components.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage ="Поле обязательно к заполнению")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        public string Password { get; set; }
    }
}
