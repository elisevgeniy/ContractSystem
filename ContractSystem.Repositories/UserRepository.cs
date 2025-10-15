using ContractSystem.Core;
using ContractSystem.Core.DTO;
using ContractSystem.Core.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace ContractSystem.Repositories
{
    public class UserRepository : IUserRepository
    {
        private DataContext _dataContext;

        public UserRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public List<UserDTO> GetAll()
        {
            return _dataContext.Users
                        .Include(u => u.Documents)
                        .Include(u => u.Approvals)
                        .ToList();
        }

        public UserDTO? GetById(int id)
        {
            return _dataContext.Users
                        .Include(u => u.Documents)
                        .Include(u => u.Approvals)
                        .Where(u => u.Id == id)
                        .FirstOrDefault();
        }

        public UserDTO? GetFirstByFirstname(string firstname)
        {
            return _dataContext.Users.Where(u => u.Firstname == firstname).SingleOrDefault(); ;
        }

        public UserDTO Add(UserDTO userDTO)
        {
            _dataContext.Users.Add(userDTO);
            _dataContext.SaveChanges();
            return userDTO;
        }

        public UserDTO Update(UserDTO userDTO)
        {
            _dataContext.Users.Update(userDTO);
            _dataContext.SaveChanges();
            return userDTO;
        }

        public void Delete(UserDTO userDTO)
        {
            _dataContext.Users.Remove(userDTO);
            _dataContext.SaveChanges();
        }
    }
}
