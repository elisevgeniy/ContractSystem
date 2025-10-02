using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractSystem.Core.Models.DTO
{
    public class ApprovalDTO
    {
        public int Id { get; set; }
        public DocumentDTO Document { get; set; }
        public UserDTO User { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public bool IsApproved { get; set; }
    }
}
