using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractSystem.Core.Models.In
{
    public class UserUpdateIn
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Поле обязательно к заполнению")]
        public string Name { get; set; }
        public string OldPassword { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; } = Role.User;
    }
}
