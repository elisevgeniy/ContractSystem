using ContractSystem.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractSystem.Core.Models.Out
{
    public class UserOut
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public List<DocumentOut> Documents { get; set; } = new List<DocumentOut>();
        public List<ApprovalOut> Approvals { get; set; } = new List<ApprovalOut>();
    }
}
