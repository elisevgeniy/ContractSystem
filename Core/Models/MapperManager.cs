using ContractSystem.Core.Models.DTO;
using ContractSystem.Core.Models.In;
using ContractSystem.Core.Models.Out;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractSystem.Core.Models
{
    public static class MapperManager
    {
        public static UserOut Map(UserDTO userDTO)
        {
            return new UserOut()
            {
                Id = userDTO.Id,
                Firstname = userDTO.Firstname,
                Lastname = userDTO.Lastname
            };
        }
        public static UserDTO Map(UserIn userIn)
        {
            return new UserDTO()
            {
                Firstname = userIn.Firstname,
                Lastname = userIn.Lastname
            };
        }
        public static DocumentOut Map(DocumentDTO documentDTO)
        {
            return new DocumentOut()
            {
                Id = documentDTO.Id,
                Index = documentDTO.Index,
                Content = documentDTO.Content,
                IsApproved = documentDTO.IsApproved
            };
        }
        public static DocumentDTO Map(DocumentIn documentIn)
        {
            return new DocumentDTO()
            {
                Index = documentIn.Index,
                Content = documentIn.Content,
                IsApproved = documentIn.IsApproved
            };
        }
        public static List<DocumentOut> Map(List<DocumentDTO> documentDTOs)
        {
            List<DocumentOut> result = new List<DocumentOut>();
            foreach (var documentDTO in documentDTOs)
            {
                result.Add(new DocumentOut()
                {
                    Index = documentDTO.Index,
                    Content = documentDTO.Content,
                    IsApproved = documentDTO.IsApproved
                });
            }
            return result;
        }
        public static ApprovalOut Map(ApprovalDTO approvalDTO)
        {
            return new ApprovalOut()
            {
                Id = approvalDTO.Id,
                ApprovalDate = approvalDTO.ApprovalDate,
                IsApproved = approvalDTO.IsApproved,
                Document = Map(approvalDTO.Document),
                User = Map(approvalDTO.User)
            };
        }
        public static List<ApprovalOut> Map(List<ApprovalDTO> approvalDTOs)
        {
            List<ApprovalOut> result = new List<ApprovalOut>();
            foreach (var approvalDTO in approvalDTOs)
            {
                result.Add(new ApprovalOut()
                {
                    Id = approvalDTO.Id,
                    ApprovalDate = approvalDTO.ApprovalDate,
                    IsApproved = approvalDTO.IsApproved,
                    Document = Map(approvalDTO.Document),
                    User = Map(approvalDTO.User)
                });
            }
            return result;
        }
        public static ApprovalDTO Map(ApprovalIn approvalIn)
        {
            return new ApprovalDTO()
            {
                ApprovalDate = approvalIn.ApprovalDate,
                IsApproved = approvalIn.IsApproved,
                Document = Map(approvalIn.Document),
                User = Map(approvalIn.User)
            };
        }
    }
}
