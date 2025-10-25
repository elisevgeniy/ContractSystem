using ContractSystem.Core.Models.Out;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ContractSystem.Core.Models.In
{
    public class DocumentUpdateIn
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Номер договора обязателен")]
        [MinLength(length: 5, ErrorMessage = "Номер договора должен быть длинее 5 символов")]
        public string Index { get; set; }
        [Required(ErrorMessage = "Содержание договора обязателено")]
        public string Content { get; set; }
        [Required(ErrorMessage = "Выбрать ответственного за договор обязателено")]
        public int OwnerId { get; set; }
        public bool IsApproved { get; set; }

        public List<ApprovalOut> Approvals { get; set; }
        public UserOut Owner { get; set; }
    }
}
