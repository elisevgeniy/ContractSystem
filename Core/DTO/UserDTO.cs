using ContractSystem.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractSystem.Core.DTO
{
    [Index(nameof(Login), Name = "IX_Login", IsUnique = true)]
    public class UserDTO
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }

        public Role Role { get; set; }

        public LoginDTO LoginData { get; set; }

        public List<DocumentDTO> Documents { get; set; } = new();

        public List<ApprovalDTO> Approvals { get; set; } = new();
    }
}
