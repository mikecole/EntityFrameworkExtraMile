using System.Data.Entity.ModelConfiguration;
using EntityFrameworkExtraMile.Domain.Model;

namespace EntityFrameworkExtraMile.Infrastructure.DataAccess.Configuration
{
    public class EmployeeConfiguration : EntityTypeConfiguration<Employee>
    {
        public EmployeeConfiguration()
        {
            HasKey(employee => employee.ID);
            Property(employee => employee.Code).HasMaxLength(20).IsRequired();
            Property(employee => employee.FirstName).HasMaxLength(50).IsRequired();
            Property(employee => employee.MiddleName).HasMaxLength(50);
            Property(employee => employee.LastName).HasMaxLength(50).IsRequired();

            HasRequired(employee => employee.Address);
            HasMany(employee => employee.PayrollDeductions);
        }
    }

    public class AddressConfiguration : EntityTypeConfiguration<Address>
    {
        public AddressConfiguration()
        {
            HasKey(address => address.ID);
            Property(address => address.AddressLine1).HasMaxLength(100).IsRequired();
            Property(address => address.AddressLine1).HasMaxLength(100);
            Property(address => address.City).HasMaxLength(100).IsRequired();
            Property(address => address.PostalCode).HasMaxLength(12).IsRequired();

            HasRequired(address => address.State);
        }
    }
    
    public class StateConfiguration : EntityTypeConfiguration<State>
    {
        public StateConfiguration()
        {
            HasKey(state => state.ID);
            Property(state => state.Abbreviation).HasMaxLength(2).IsRequired();
            Property(state => state.Name).HasMaxLength(50).IsRequired();

            HasMany(state => state.StateFor);
        }
    }

    public class DepartmentConfiguration : EntityTypeConfiguration<Department>
    {
        public DepartmentConfiguration()
        {
            HasKey(department => department.ID);
            Property(department => department.Name).HasMaxLength(50).IsRequired();
            Property(department => department.Code).HasMaxLength(10).IsRequired();

            HasMany(department => department.Employees);
        }
    }

    public class PayrollDeductionConfiguration : EntityTypeConfiguration<PayrollDeduction>
    {
        public PayrollDeductionConfiguration()
        {
            HasKey(deduction => deduction.ID);
            Property(deduction => deduction.Name).HasMaxLength(50).IsRequired();

            HasMany(deduction => deduction.Employees);
        }
    }

    public class UserProfileConfiguration : EntityTypeConfiguration<UserProfile>
    {
        public UserProfileConfiguration()
        {
            HasKey(prof => prof.ID);
            Property(prof => prof.UserName).HasMaxLength(50).IsRequired();
        }
    }
}