using ContractSystem.Core;
using ContractSystem.Core.DTO;
using ContractSystem.Core.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractSystem.Repositories
{
    public class ApprovalRepository : IApprovalRepository
    {
        private DataContext _dataContext;

        public ApprovalRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public ApprovalDTO Add(ApprovalDTO approvalDTO)
        {
            _dataContext.Approvals.Add(approvalDTO);
            _dataContext.SaveChanges();
            return approvalDTO;
        }

        public void Delete(ApprovalDTO approvalDTO)
        {
            _dataContext.Approvals.Remove(approvalDTO);
            _dataContext.SaveChanges();
        }

        public List<ApprovalDTO> GetAll()
        {
            return _dataContext.Approvals.Include(a => a.User).Include(a => a.Document).ToList();
        }

        public List<ApprovalDTO> GetAllByUser(UserDTO userDTO)
        {
            return _dataContext.Approvals.Include(a => a.User).Include(a => a.Document).Where(a => a.User.Id == userDTO.Id).ToList();
        }

        public ApprovalDTO? GetById(int id)
        {
            return _dataContext.Approvals.Find(id);
        }

        public ApprovalDTO Update(ApprovalDTO approvalDTO)
        {
            _dataContext.Update(approvalDTO);
            _dataContext.SaveChanges();
            return approvalDTO;
        }
    }
}
