using ContractSystem.Core.Models;

namespace ContractSystem.WebApp.Components.Models
{
    public class AuthedUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Role Role { get; set; }
    }
}
