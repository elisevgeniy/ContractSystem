using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractSystem.Core.Models.Out
{
    public class DocumentOut
    {
        public int Id { get; set; }
        public string Index { get; set; }
        public string Content { get; set; }
        public bool IsApproved { get; set; }
    }
}
