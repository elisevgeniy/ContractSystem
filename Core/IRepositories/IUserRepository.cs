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
        List<UserDTO> GetAll();
    }
}
