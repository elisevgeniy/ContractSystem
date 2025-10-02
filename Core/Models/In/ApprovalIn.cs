using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractSystem.Core.Models.In
{
    public class ApprovalIn
    {
        public int Id { get; set; }
        public DocumentIn Document { get; set; }
        public UserIn User { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public bool IsApproved { get; set; }
    }
}
