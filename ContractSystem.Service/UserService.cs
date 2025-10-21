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

        public UserOut getById(int id)
        {
            var userDTO = _userRepository.GetById(id);
            var result = userDTO.Adapt<UserOut>();
            return result;
        }

        public UserOut getByLogin(string login)
        {
            var userDTO = _userRepository.GetByLogin(login);
            var result = userDTO.Adapt<UserOut>();
            return result;
        }

        public List<UserOut> getAll()
        {
            var userDTOs = _userRepository.GetAll();
            var result = userDTOs.Adapt<List<UserOut>>();
            return result;
        }

        public UserOut AddUser(UserIn userIn)
        {
            UserOut? userOut = null;
            UserDTO? userDTO = _userRepository.GetByLogin(userIn.Login);
            if (userDTO != null) throw new Exception("Такой пользователь уже существует");

            userDTO = new UserDTO()
            {
                Login = userIn.Login,
                Name = userIn.Name,
                Role = userIn.Role,
                LoginData = new LoginDTO()
                {
                    Password = userIn.Password,
                }
            };
            InicializeUser(userDTO);
            userDTO = _userRepository.Add(userDTO);


            userOut = userDTO.Adapt<UserOut>();

            return userOut;
        }
        public UserOut? GetUser(string login)
        {
            UserDTO? userDTO = _userRepository.GetByLogin(login);

            if (userDTO == null) return null;
            return userDTO.Adapt<UserOut>();
        }

        public bool Auth(LoginIn loginIn)
        {
            UserDTO? userDTO = _userRepository.GetByLogin(loginIn.Login);
            if (userDTO == null) return false;
            return userDTO.LoginData.Password.Equals(loginIn.Password);
        }

        private static void InicializeUser(UserDTO userDTO)
        {
            for (int i = 0; i < 4; i++)
            {
                var doc = new DocumentDTO()
                {
                    Index = $"Doc-{userDTO.Login}-{i + 1}",
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