using ContractSystem.Core.DTO;
using ContractSystem.Core.IRepositories;
using ContractSystem.Core.Models.In;
using ContractSystem.Core.Models.Out;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractSystem.Service
{
    public class ApprovalService
    {
        private IApprovalRepository _approvalRepository;

        public ApprovalService(IApprovalRepository approvalRepository)
        {
            _approvalRepository = approvalRepository;
        }

        public ApprovalOut? Get(int id)
        {
            return _approvalRepository.GetById(id).Adapt<ApprovalOut>();
        }

        public List<ApprovalOut> GetAll()
        {
            return _approvalRepository.GetAll().Adapt<List<ApprovalOut>>();
        }

        public ApprovalOut? GetByUserAndDocument(int userId, int documentId)
        {
            return _approvalRepository.GetByUserAndDocument(userId, documentId).Adapt<ApprovalOut>();
        }

        public List<ApprovalOut> GetAllByUser(int userId)
        {
            return _approvalRepository.GetAllByUser(userId).Adapt<List<ApprovalOut>>();
        }

        public List<ApprovalOut> GetAllByDocument(int documentId)
        {
            return _approvalRepository.GetAllByDocument(documentId).Adapt<List<ApprovalOut>>();
        }
        public void Approve(ApprovalIn approvalIn)
        {
            ApprovalDTO approvalDTO = _approvalRepository.GetAllByUser(approvalIn.UserId).Where(a => a.DocumentId == approvalIn.DocumentId).FirstOrDefault();
            approvalDTO.IsApproved = approvalIn.IsApproved;
            _approvalRepository.Update(approvalDTO);
        }
        public ApprovalOut Approve(int approvalId)
        {
            ApprovalDTO? approvalDTO = _approvalRepository.GetById(approvalId);
            if (approvalDTO == null) throw new Exception("Согласование не найдено");
            approvalDTO.IsApproved = true;
            approvalDTO.ApprovalDate = DateTime.UtcNow;
            return _approvalRepository.Update(approvalDTO).Adapt<ApprovalOut>();
        }
    }
}
