using System.Data.Entity;
using EntityFrameworkExtraMile.Domain.Model;
using EntityFrameworkExtraMile.Infrastructure.DataAccess.Configuration;

namespace EntityFrameworkExtraMile.Infrastructure.DataAccess
{
    public class HumanResourceContext : DbContext
    {
        public HumanResourceContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<PayrollDeduction> PayrollDeductions { get; set; }
        public DbSet<CompanyAsset> CompanyAssets { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new EmployeeConfiguration());
            modelBuilder.Configurations.Add(new AddressConfiguration());
            modelBuilder.Configurations.Add(new StateConfiguration());
            modelBuilder.Configurations.Add(new DepartmentConfiguration());
            modelBuilder.Configurations.Add(new UserProfileConfiguration());
            modelBuilder.Configurations.Add(new CompanyAssetConfiguration());
            modelBuilder.Configurations.Add(new PayrollDeductionConfiguration());
        }
    }
}