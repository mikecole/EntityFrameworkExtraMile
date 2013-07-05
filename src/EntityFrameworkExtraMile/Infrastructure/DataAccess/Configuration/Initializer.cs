using System.Data.Entity;
using System.Linq;
using EntityFrameworkExtraMile.Domain.Model;

namespace EntityFrameworkExtraMile.Infrastructure.DataAccess.Configuration
{
    public class HumanResourceInitializer : DropCreateDatabaseAlways<HumanResourceContext>
    {
        protected override void Seed(HumanResourceContext context)
        {
            base.Seed(context);

            SeedState(context, new[] { "AL", "Alabama" }, new[] { "AK", "Alaska" }, new[] { "AZ", "Arizona" }, new[] { "AR", "Arkansas" }, new[] { "CA", "California" }, new[] { "CO", "Colorado" }, new[] { "CT", "Connecticut" }, new[] { "DE", "Delaware" }, new[] { "DC", "District of Columbia" }, new[] { "FL", "Florida" }, new[] { "GA", "Georgia" }, new[] { "HI", "Hawaii" }, new[] { "ID", "Idaho" }, new[] { "IL", "Illinois" }, new[] { "IN", "Indiana" }, new[] { "IA", "Iowa" }, new[] { "KS", "Kansas" }, new[] { "KY", "Kentucky" }, new[] { "LA", "Louisiana" }, new[] { "ME", "Maine" }, new[] { "MD", "Maryland" }, new[] { "MA", "Massachusetts" }, new[] { "MI", "Michigan" }, new[] { "MN", "Minnesota" }, new[] { "MS", "Mississippi" }, new[] { "MO", "Missouri" }, new[] { "MT", "Montana" }, new[] { "NE", "Nebraska" }, new[] { "NV", "Nevada" }, new[] { "NH", "New Hampshire" }, new[] { "NJ", "New Jersey" }, new[] { "NM", "New Mexico" }, new[] { "NY", "New York" }, new[] { "NC", "North Carolina" }, new[] { "ND", "North Dakota" }, new[] { "OH", "Ohio" }, new[] { "OK", "Oklahoma" }, new[] { "OR", "Oregon" }, new[] { "PA", "Pennsylvania" }, new[] { "RI", "Rhode Island" }, new[] { "SC", "South Carolina" }, new[] { "SD", "South Dakota" }, new[] { "TN", "Tennessee" }, new[] { "TX", "Texas" }, new[] { "UT", "Utah" }, new[] { "VT", "Vermont" }, new[] { "VA", "Virginia" }, new[] { "WA", "Washington" }, new[] { "WV", "West Virginia" }, new[] { "WI", "Wisconsin" }, new[] { "WY", "Wyoming" });
            SeedDepartments(context, new[] { "IT", "Information Technologies" }, new[] { "Sales", "Sales" }, new[] { "Acct", "Accounting" }, new[] { "R&D", "Research and Development" });
        }

        private void SeedState(HumanResourceContext context, params string[][] states)
        {
            for (var index = 0; index < states.Length; index++)
            {
                var item = states[index];
                if (context.States.All(state => state.Abbreviation != item[0]))
                {
                    context.States.Add(new State(item[0], item[1]));
                    context.SaveChanges();
                }
            }
        }

        private void SeedDepartments(HumanResourceContext context, params string[][] departments)
        {
            for (var index = 0; index < departments.Length; index++)
            {
                var item = departments[index];
                if (context.Departments.All(dept => dept.Code != item[0]))
                {
                    context.Departments.Add(new Department(item[0], item[1]));
                    context.SaveChanges();
                }
            }
        }
    }
}