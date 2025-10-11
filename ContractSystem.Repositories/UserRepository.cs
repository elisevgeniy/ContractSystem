using ContractSystem.Core;
using ContractSystem.Core.DTO;
using ContractSystem.Core.IRepositories;

namespace ContractSystem.Repositories
{
    public class UserRepository: IUserRepository
    {
        private DataContext _dataContext;

        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public List<UserDTO> GetAll()
        {
            return _dataContext.Users.ToList();
        }
    }
}
