using ContractSystem.Core.Models;
using ContractSystem.Core.Models.DTO;
using ContractSystem.Core.Models.In;
using ContractSystem.Core.Models.Out;
using ContractSystem.RepositoryOld;

namespace ContractSystem.Service
{
    public class UserService
    {
        public static UserOut AddUser(string firstname, string lastname)
        {
            UserOut user = null;
            UserDTO userDTO = UserRepository.GetFirstUserByFirstname(firstname);
            if (userDTO == null)
            {
                user = MapperManager.Map(UserRepository.AddUser(firstname, lastname));
                InicializeUser(user);
            } else
            {
                user = MapperManager.Map(userDTO);
            }

                return user;
        }
        public static UserOut? GetUser(string firstname)
        {
            UserDTO userDTO = UserRepository.GetFirstUserByFirstname(firstname);

            if (userDTO == null) return null;
            return MapperManager.Map(userDTO);
        }

        private static void InicializeUser(UserOut user)
        {
            for (int i = 0; i < 4; i++)
            {
                var doc = DocumentRepository.AddDocument(
                    MapperManager.Map(new DocumentIn() { Index = $"Doc-{user.Firstname}-{i + 1}", Content = "Some content" }
                    ));
                DocumentRepository.MakeOwnedDocumentToUser(user.Id, doc.Id);
                if (i % 2 == 0)
                {
                    ApprovalRepository.AddApproval(user.Id, doc.Id);
                }
            }
        }
    }
}