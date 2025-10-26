using ContractSystem.Core.DTO;
using ContractSystem.Core.Exceptions;
using ContractSystem.Core.IRepositories;
using ContractSystem.Core.Models;
using ContractSystem.Core.Models.In;
using ContractSystem.Core.Models.Out;
using ContractSystem.Core.Models.Search;
using ContractSystem.Repositories;
using Mapster;
using System.Linq;

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
            try
            {
                var docDTO = _documentRepository.GetById(id);
                return docDTO.Adapt<DocumentOut>();
            } catch (Exception e)
            {
                throw new NotFoundException("Документ не найден", e);
            }
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
            var user = _userService.getById(documentIn.OwnerId);

            var docDTO = documentIn.Adapt<DocumentDTO>();
            docDTO = _documentRepository.Add(docDTO);
            return docDTO.Adapt<DocumentOut>();
        }
        public DocumentOut Update(DocumentUpdateIn document)
        {
            var user = _userService.getById(document.OwnerId);

            var docDTO = _documentRepository.GetById(document.Id);
            docDTO.Index = document.Index;
            docDTO.Content = document.Content;

            List<ApprovalDTO> toRemove = new();

            foreach(var appr in docDTO.Approvals)
            {
                if (!document.ApprovalUsers.Contains(appr.User.Adapt<UserOut>()))
                {
                    toRemove.Add(appr);
                }
            }
            docDTO.Approvals.RemoveAll(a => toRemove.Contains(a));

            foreach(var apprUser in document.ApprovalUsers)
            {
                if (docDTO.Approvals.Find(a => a.UserId == apprUser.Id) == null)
                {
                    docDTO.Approvals.Add(new ApprovalDTO()
                    {
                        DocumentId = document.Id,
                        UserId = apprUser.Id,
                    });
                }
            }
            
            docDTO = _documentRepository.Update(docDTO);
            return docDTO.Adapt<DocumentOut>();
        }
        public void Delete(int documentId)
        {
            var docDTO = _documentRepository.GetById(documentId);
            _documentRepository.Delete(docDTO);
        }
    }
}