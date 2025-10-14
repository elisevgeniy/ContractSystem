using ContractSystem.Core.DTO;
using ContractSystem.Core.IRepositories;
using ContractSystem.Core.Models;
using ContractSystem.Core.Models.In;
using ContractSystem.Core.Models.Out;
using ContractSystem.Repositories;
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

        public UserOut AddUser(string firstname, string lastname)
        {
            UserOut? userOut = null;
            UserDTO? userDTO = _userRepository.GetFirstByFirstname(firstname);
            if (userDTO == null)
            {
                userDTO = new UserDTO()
                {
                    Firstname = firstname,
                    Lastname = lastname
                };
                InicializeUser(userDTO);
                userDTO = _userRepository.Add(userDTO);
            }

            userOut = userDTO.Adapt<UserOut>();

            return userOut;
        }
        public UserOut? GetUser(string firstname)
        {
            UserDTO? userDTO = _userRepository.GetFirstByFirstname(firstname);

            if (userDTO == null) return null;
            return userDTO.Adapt<UserOut>();
        }

        private static void InicializeUser(UserDTO userDTO)
        {
            for (int i = 0; i < 4; i++)
            {
                var doc = new DocumentDTO()
                {
                    Index = $"Doc-{userDTO.Firstname}-{i + 1}",
                    Content = "Some content",
                    Owner = userDTO
                };
                userDTO.Documents.Add(doc);
                if (i % 2 == 0)
                {
                    userDTO.Approvals.Add(
                        new ApprovalDTO()
                        {
                            User = userDTO,
                            Document = doc
                        }
                    );
                }
            }
        }
    }
}