using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractSystem.Core.DTO
{
    public class Approval
    {
        public int Id { get; set; }
        public Document document { get; set; }
        public User user { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public bool IsApproved { get; set; }
    }
}
