using ContractSystem.Core.Models;
using ContractSystem.Core.DTO;
using ContractSystem.Core.Models.In;
using ContractSystem.Core.Models.Out;
using ContractSystem.RepositoryOld;

namespace ContractSystem.Service
{
    public class DocumentService
    {
        public static List<DocumentOut> GetAllDocumentsByUser(int user_id)
        {
            return MapperManager.Map(DocumentRepository.GetAllDocumentsByUser(user_id));
        }
        public static List<DocumentOut> GetDocumentForApproveByUser(int user_id)
        {
            var result = new List<DocumentOut>();
            List<ApprovalOut> approvals = MapperManager.Map(ApprovalRepository.GetApprovalsByUser(user_id));
            result = approvals.Where(approval => !approval.Document.IsApproved).Select(approval => approval.Document).ToList();
            return result;
        }
        public static DocumentOut AddDocumentByUser(string index, string content, int user_id)
        {
            var doc = MapperManager.Map(DocumentRepository.AddDocument(
                MapperManager.Map(new DocumentIn()
                {
                    Content = content,
                    Index = index
                }
                )));
            DocumentRepository.MakeOwnedDocumentToUser(user_id, doc.Id);
            ApprovalRepository.AddApproval(user_id, doc.Id);
            return doc;
        }
        public static bool Approve(int documentId, int user_id)
        {
            return ApprovalRepository.UpdateApproval(user_id, documentId, true);
        }
    }
}