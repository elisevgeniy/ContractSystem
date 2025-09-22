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
    }
}