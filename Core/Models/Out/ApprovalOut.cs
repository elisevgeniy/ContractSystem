using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ContractSystem.Core.Models.Out
{
    public class ApprovalOut
    {
        public int Id { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public bool IsApproved { get; set; }
        public int UserId { get; set; }
        public int DocumentId { get; set; }

        [JsonIgnore]
        public DocumentOut Document { get; set; }
        [JsonIgnore]
        public UserOut User { get; set; }
    }
}
