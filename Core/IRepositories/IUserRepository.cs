using ContractSystem.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractSystem.Core.IRepositories
{
    public interface IUserRepository
    {
        public List<UserDTO> GetAll();

        public UserDTO? GetById(int id);

        public UserDTO? GetFirstByFirstname(string firstname);

        public UserDTO Add(UserDTO userDTO);

        public UserDTO Update(UserDTO userDTO);

        public void Delete(UserDTO userDTO);
    }
}
