using ContractSystem.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ContractSystem.Core.Models.Out
{
    public class UserOut
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public Role Role { get; set; }
        [JsonIgnore]
        public List<DocumentOut> Documents { get; set; } = new List<DocumentOut>();
        [JsonIgnore]
        public List<ApprovalOut> Approvals { get; set; } = new List<ApprovalOut>();
    }
}
