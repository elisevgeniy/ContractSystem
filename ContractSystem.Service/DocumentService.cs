using ContractSystem.Core.DTO;
using ContractSystem.Repository;

namespace ContractSystem.Service
{
    public class DocumentService
    {
        public static List<Document> GetDocumentByUser(User user)
        {
            return DocumentRepository.GetAllDocumentsByUser(user.Id);
        }
        public static List<Document> GetDocumentForApproveByUser(User user)
        {
            var result = new List<Document>();
            var approvals = ApprovalRepository.GetApprovalsByUser(user.Id);
            foreach (var approval in approvals)
            {
                if (!approval.Document.IsApproved) 
                    result.Add(approval.Document);
            }
            return result;
        }
        public static Document AddDocumentByUser(string index, string content, User user)
        {
            var doc = DocumentRepository.AddDocument(index, content);
            DocumentRepository.MakeOwnedDocumentToUser(user.Id, doc.Id);
            ApprovalRepository.AddApproval(user.Id, doc.Id);
            return doc;
        }
        public static bool Approve(int documentId, User user)
        {
            return ApprovalRepository.UpdateApproval(user.Id, documentId, true);
        }
    }
}