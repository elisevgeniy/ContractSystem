using ContractSystem.Core.DTO;
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

        public DocumentService(IDocumentRepository documentRepository, IApprovalRepository approvalRepository)
        {
            _documentRepository = documentRepository;
            _approvalRepository = approvalRepository;
        }
        public List<DocumentOut> GetAll()
        {
            var docDTOs = _documentRepository.GetAll();
            return docDTOs.Adapt<List<DocumentOut>>();
        }

        public List<DocumentOut> GetAllDocumentsByUser(UserSearch userSearch)
        {
            var userDTO = userSearch.Adapt<UserDTO>();
            var docDTOs = _documentRepository.GetAllByUser(userDTO);
            return docDTOs.Adapt<List<DocumentOut>>();
        }
        public List<DocumentOut> GetDocumentForApproveByUser(UserSearch userSearch)
        {
            return _approvalRepository.GetAllByUser(userSearch.Adapt<UserDTO>())
                                        .Select(a => a.Document.Adapt<DocumentOut>())
                                        .ToList();
        }
        public DocumentOut AddDocument(DocumentIn documentIn)
        {
            var docDTO = documentIn.Adapt<DocumentDTO>();
            docDTO = _documentRepository.Add(docDTO);
            return docDTO.Adapt<DocumentOut>(); // TODO: Разобраться, почему падает Mapster
        }
        public void Approve(ApprovalSearch approvalSearch)
        {
            _approvalRepository.Update(new ApprovalDTO()
            {
                UserId = approvalSearch.User.Id,
                DocumentId = approvalSearch.Document.Id,
                IsApproved = true
            });
        }
    }
}