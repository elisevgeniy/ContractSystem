using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractSystem.Core.DTO
{
    [Index(nameof(UserId), nameof(DocumentId), Name = "IX_UserIdAndDocumentId", IsUnique = true)]
    public class ApprovalDTO
    {
        public int Id { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public bool IsApproved { get; set; }

        [ForeignKey("FK_Approvals_Users_UserId")]
        public int UserId { get; set; }

        [ForeignKey("FK_Approvals_Documents_DocumentId")]
        public int DocumentId { get; set; }

        public DocumentDTO Document { get; set; }
        public UserDTO User { get; set; }
    }
}
