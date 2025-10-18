using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractSystem.Core.Models.In
{
    public class ApprovalIn
    {
        public int DocumentId { get; set; }
        public int UserId { get; set; }
        public bool IsApproved { get; set; }
    }
}
