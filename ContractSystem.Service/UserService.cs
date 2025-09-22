using ContractSystem.Core.DTO;
using ContractSystem.Repository;

namespace ContractSystem.Service
{
    public class UserService
    {
        public static User AddUser(string firstname, string lastname)
        {
            User? user = UserRepository.GetFirstUserByFirstname(firstname);
            if (user == null)
            {
                user = UserRepository.AddUser(firstname, lastname);
                InicializeUser(user);
            }

            return user;
        }
        public static User? GetUser(string firstname)
        {
            User? user = UserRepository.GetFirstUserByFirstname(firstname);
            return user;
        }

        private static void InicializeUser(User user)
        {
            for (int i = 0; i < 4; i++)
            {
                var doc = DocumentRepository.AddDocument($"Doc-{user.Firstname}-{i + 1}", "Some content");
                DocumentRepository.MakeOwnedDocumentToUser(user.Id, doc.Id);
                if (i % 2 == 0)
                {
                    ApprovalRepository.AddApproval(user.Id, doc.Id);
                }
            }
        }
    }
}