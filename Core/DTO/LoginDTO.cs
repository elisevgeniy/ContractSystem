using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractSystem.Core.DTO
{
    public class LoginDTO
    {
        public int Id {  get; set; }
        public string Password { get; set; }

        [ForeignKey("FK_Logins_Users_UserId")]
        public int UserId { get; set; } 

        public UserDTO User { get; set; }
    }
}
