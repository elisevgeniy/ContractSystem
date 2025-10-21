using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractSystem.Core.Models.In
{
    public class LoginIn
    {
        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        public string Password { get; set; }
    }
}
