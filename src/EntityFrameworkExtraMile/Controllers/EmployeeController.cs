using System.Linq;
using System.Web.Mvc;
using EntityFrameworkExtraMile.Infrastructure.DataAccess;

namespace EntityFrameworkExtraMile.Controllers
{
    public class EmployeeController : Controller
    {
        //
        // GET: /Employee/

        public ActionResult Index()
        {
            var context = new HumanResourceContext();

            var employees = context.Employees.ToArray();

            #region hide
            //var employees = context.Employees
            //           .Include(e => e.Address.State)
            //           .Include(e => e.PayrollDeductions)
            //           .Include(e => e.CompanyAssets)
            //           .ToArray();

            //var employees = context.Employees
            //                       .Include(e => e.Address.State)
            //                       .ToArray();

            //employees = context.Employees
            //       .Include(e => e.PayrollDeductions)
            //       .ToArray();

            //employees = context.Employees
            //       .Include(e => e.CompanyAssets)
            //       .ToArray();
            #endregion

            return View(employees);
        }
    }
}