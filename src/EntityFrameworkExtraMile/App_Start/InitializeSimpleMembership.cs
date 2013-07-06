using EntityFrameworkExtraMile.App_Start;
using WebActivatorEx;
using WebMatrix.WebData;

[assembly: PostApplicationStartMethod(typeof(InitializeSimpleMembership), "PostStart")]
namespace EntityFrameworkExtraMile.App_Start
{
    public class InitializeSimpleMembership
    {
        public static void PostStart()
        {
            if (!WebSecurity.Initialized)
            {
                WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfiles", "ID", "UserName", true);
            }
        }
    }
}