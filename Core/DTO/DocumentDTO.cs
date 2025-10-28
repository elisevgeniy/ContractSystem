using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractSystem.Core.DTO
{
    [Index(nameof(Index), Name = "IX_Index", IsUnique = true)]
    public class DocumentDTO
    {
        public int Id { get; set; }
        public string Index { get; set; }
        public string Content { get; set; }
        public bool IsApproved { get; set; }

        [ForeignKey("FK_Documents_Users_OwnerId")]
        public int OwnerId { get; set; }

        public List<ApprovalDTO> Approvals { get; set; } = new ();

        public UserDTO Owner { get; set; }
    }
}
