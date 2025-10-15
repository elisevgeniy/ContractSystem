using ContractSystem.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractSystem.Core.IRepositories
{
    public interface IApprovalRepository
    {
        public List<ApprovalDTO> GetAll();

        public List<ApprovalDTO> GetAllByUser(UserDTO userDTO);

        public ApprovalDTO? GetById(int id);

        public ApprovalDTO Add(ApprovalDTO approvalDTO);

        public ApprovalDTO Update(ApprovalDTO approvalDTO);

        public void Delete(ApprovalDTO approvalDTO);
    }
}
