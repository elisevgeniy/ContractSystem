using ContractSystem.Core.DTO;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ContractSystem.Core
{
    public class DataContext:DbContext
    {
        public DataContext()
        {
            Database.EnsureCreated();
        }

        public DbSet<UserDTO> Users { get; set; }

        public DbSet<DocumentDTO> Documents { get; set; }

        public DbSet<ApprovalDTO> Approvals { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Options.ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
