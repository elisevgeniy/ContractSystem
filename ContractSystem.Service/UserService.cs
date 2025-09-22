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
            }

            return user;
        }
    }
}