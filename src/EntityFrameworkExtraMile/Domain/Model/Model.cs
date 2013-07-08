using System;
using System.Collections.Generic;

namespace EntityFrameworkExtraMile.Domain.Model
{
    public abstract class EntityBase
    {
        public int ID { get; set; }
    }

    public enum Genders
    {
        Male,
        Female
    }

    public class Employee : EntityBase
    {
        public string Code { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Genders Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime HireDate { get; set; }

        public virtual Address Address { get; set; }
        public virtual Department Department { get; set; }
        public virtual ICollection<PayrollDeduction> PayrollDeductions { get; set; }
        public virtual ICollection<CompanyAsset> CompanyAssets { get; set; } 
    }

    public class Address : EntityBase
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }

        public virtual State State { get; set; }
        public virtual ICollection<Employee> AddressFor { get; set; }
    }

    public class State : EntityBase
    {
        public State(string abbreviation, string name)
            : this()
        {
            Abbreviation = abbreviation;
            Name = name;
        }
        private State()
        {
        }

        public string Abbreviation { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Address> StateFor { get; set; }
    }

    public class Department : EntityBase
    {
        public Department(string code, string name)
            :this()
        {
            Code = code;
            Name = name;
        }
        private Department()
        {
        }
        
        public string Name { get; set; }
        public string Code { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }

    public class PayrollDeduction : EntityBase
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }

        public virtual ICollection<Employee> Employees { get; set; } 
    }

    public class CompanyAsset : EntityBase
    {
        public string Name { get; set; }

        public virtual ICollection<Employee> Employees { get; set; } 
    }

    public class UserProfile : EntityBase
    {
        public string UserName { get; set; }
    }
}