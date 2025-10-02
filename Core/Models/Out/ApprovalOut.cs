using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractSystem.Core.Models.Out
{
    public class ApprovalOut
    {
        public int Id { get; set; }
        public DocumentOut Document { get; set; }
        public UserOut User { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public bool IsApproved { get; set; }
    }
}
