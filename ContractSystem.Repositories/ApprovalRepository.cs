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

        public List<ApprovalDTO> GetAllByUser(int userId)
        {
            return _dataContext.Approvals.Include(a => a.User).Include(a => a.Document).Where(a => a.User.Id == userId).ToList();
        }

        public ApprovalDTO? GetByUserAndDocument(int userId, int documentId)
        {
            return _dataContext.Approvals.Include(a => a.User).Include(a => a.Document).Where(a => a.UserId == userId && a.DocumentId == documentId).FirstOrDefault();
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
