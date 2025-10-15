using ContractSystem.Core.Models.Out;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractSystem.Core.Models.Search
{
    public class ApprovalSearch
    {
        public int Id { get; set; }
        public DocumentSearch Document { get; set; }
        public UserSearch User { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public bool IsApproved { get; set; }
    }
}
