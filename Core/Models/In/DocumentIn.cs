using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractSystem.Core.Models.In
{
    public class DocumentIn
    {
        public string Index { get; set; }
        public string Content { get; set; }
        public bool IsApproved { get; set; }
    }
}
