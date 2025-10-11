using ContractSystem.Core.DTO;
using ContractSystem.Core.IRepositories;
using ContractSystem.Core.Models;
using ContractSystem.Core.Models.In;
using ContractSystem.Core.Models.Out;
using ContractSystem.RepositoryOld;
using Mapster;

namespace ContractSystem.Service
{
    public class UserService
    {
        private IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<UserOut> getAll()
        {
            var userDTOs = _userRepository.GetAll();
            var result = userDTOs.Adapt<List<UserOut>>();
            return result;
        }

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