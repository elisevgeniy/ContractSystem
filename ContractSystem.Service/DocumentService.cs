using ContractSystem.Core.DTO;
using ContractSystem.Core.Exceptions;
using ContractSystem.Core.IRepositories;
using ContractSystem.Core.Models;
using ContractSystem.Core.Models.In;
using ContractSystem.Core.Models.Out;
using ContractSystem.Core.Models.Search;
using ContractSystem.Repositories;
using Mapster;

namespace ContractSystem.Service
{
    public class DocumentService
    {
        private IDocumentRepository _documentRepository;
        private IApprovalRepository _approvalRepository;
        private UserService _userService;

        public DocumentService(IDocumentRepository documentRepository, IApprovalRepository approvalRepository, UserService userService)
        {
            _documentRepository = documentRepository;
            _approvalRepository = approvalRepository;
            _userService = userService;
        }
        public DocumentOut GetById(int id)
        {
            var docDTO = _documentRepository.GetById(id);
            return docDTO.Adapt<DocumentOut>();
        }
        public List<DocumentOut> GetAll()
        {
            var docDTOs = _documentRepository.GetAll();
            return docDTOs.Adapt<List<DocumentOut>>();
        }

        public List<DocumentOut> GetAllDocumentsByUser(int userId)
        {
            var docDTOs = _documentRepository.GetAllByUser(userId);
            return docDTOs.Adapt<List<DocumentOut>>();
        }
        public List<DocumentOut> GetDocumentForApproveByUser(int userId)
        {
            return _approvalRepository.GetAllByUser(userId)
                                        .Select(a => a.Document.Adapt<DocumentOut>())
                                        .ToList();
        }
        public DocumentOut AddDocument(DocumentIn documentIn)
        {
            if (_userService.getById(documentIn.OwnerId) == null)
                throw new NotFoundException("Пользователь не найден");

            var docDTO = documentIn.Adapt<DocumentDTO>();
            docDTO = _documentRepository.Add(docDTO);
            return docDTO.Adapt<DocumentOut>(); // TODO: Разобраться, почему падает Mapster
        }
        public void Approve(ApprovalIn approvalIn)
        {
            ApprovalDTO approvalDTO = _approvalRepository.GetAllByUser(approvalIn.UserId).Where(a => a.DocumentId == approvalIn.DocumentId).FirstOrDefault();
            approvalDTO.IsApproved = approvalIn.IsApproved;
            _approvalRepository.Update(approvalDTO);
        }
        public void Approve(int approvalId)
        {
            ApprovalDTO? approvalDTO = _approvalRepository.GetById(approvalId);
            if (approvalDTO == null) throw new Exception("Согласование не найдено");
            approvalDTO.IsApproved = true;
            approvalDTO.ApprovalDate = new DateTime();
            _approvalRepository.Update(approvalDTO);
        }
    }
}