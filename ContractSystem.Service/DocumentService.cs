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
        public static Document AddDocumentByUser(string index, string content, User user)
        {
            var doc = DocumentRepository.AddDocument(index, content);
            DocumentRepository.MakeOwnedDocumentToUser(user.Id, doc.Id);
            ApprovalRepository.AddApproval(user.Id, doc.Id);
            return doc;
        }
    }
}