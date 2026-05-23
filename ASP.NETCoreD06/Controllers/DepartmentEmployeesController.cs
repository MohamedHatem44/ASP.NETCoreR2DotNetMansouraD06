using ASP.NETCoreD06.Data.Context;
using ASP.NETCoreD06.ViewModels.DepartmentEmployees;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ASP.NETCoreD06.Controllers
{
    public class DepartmentEmployeesController : Controller
    {
        /*------------------------------------------------------------------*/
        private readonly AppDbContext db = new AppDbContext();
        /*------------------------------------------------------------------*/
        public IActionResult Index()
        {
            var departmentsForDropDown = GetDepartmentsForDropDown();
            var departmentsForDropDownVM = new DepartmentsForDropDown
            {
                Departments = departmentsForDropDown
            };
            return View(departmentsForDropDownVM);
        }
        /*------------------------------------------------------------------*/
        [HttpGet]
        public IActionResult GetEmployeesByDepartmentId(int departmentId)
        {
            var employeesVM = db.Employees
                .Include(e => e.Department)
                .Where(e => e.DepartmentId == departmentId)
                .Select(e => new DepartmentEmployeesReadVM
                {
                    Id = e.Id,
                    Name = e.Name,
                    Age = e.Age,
                    Salary = e.Salary,
                    Department = e.Department.Name
                }).ToList();

            return PartialView("_GetEmployeesByDepartmentId", employeesVM); // No Layout
            //return View("_EmployeesTablePartial", employeesVM); // Layout
        }
        /*------------------------------------------------------------------*/
        private List<SelectListItem> GetDepartmentsForDropDown()
        {
            return db.Departments
             .Select(d => new SelectListItem
             {
                 Value = d.Id.ToString(),
                 Text = d.Name
             }).ToList();
        }
        /*------------------------------------------------------------------*/
    }
}
