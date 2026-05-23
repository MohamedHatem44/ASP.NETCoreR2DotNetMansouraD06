using ASP.NETCoreD06.Data.Context;
using ASP.NETCoreD06.Models;
using ASP.NETCoreD06.ViewModels.Employee;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ASP.NETCoreD06.Controllers
{
    public class EmployeeController : Controller
    {
        /*------------------------------------------------------------------*/
        private readonly AppDbContext db = new AppDbContext();
        /*------------------------------------------------------------------*/
        [HttpGet]
        public IActionResult Index()
        {
            // Get All Employees
            // Map From Domain Model To VM
            var employeesReadVM = db.Employees
                .Include(e => e.Department)
                .Select(e => new EmployeeReadVM
                {
                    Id = e.Id,
                    Name = e.Name,
                    Age = e.Age,
                    Salary = e.Salary,
                    ImageURL = e.ImageUrl,
                    Department = e.Department!.Name
                }).ToList();

            return View(employeesReadVM);
        }
        /*------------------------------------------------------------------*/
        // View Details
        [HttpGet]
        public IActionResult Details(int id)
        {
            var employee = db.Employees
                .Include(e => e.Department)
                .FirstOrDefault(e => e.Id == id);

            if (employee == null)
            {
                return RedirectToAction("Index");
            }

            // Map FromDomain Model To View Model
            var employeeReadVM = new EmployeeReadVM
            {
                Id = employee.Id,
                Name = employee.Name,
                Age = employee.Age,
                Salary = employee.Salary,
                Department = employee.Department!.Name
            };

            return View(employeeReadVM);
        }
        /*------------------------------------------------------------------*/
        [HttpGet]
        public IActionResult Create()
        {
            var employeeCreateVM = new EmployeeCreateVM
            {
                Departments = GetDepartmentsForDropDown()
            };
            return View(employeeCreateVM);
        }
        /*------------------------------------------------------------------*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EmployeeCreateVM employeeCreateVM)
        {
            // Validation
            if (!ModelState.IsValid)
            {
                employeeCreateVM = new EmployeeCreateVM
                {
                    Departments = GetDepartmentsForDropDown()
                };
                return View(employeeCreateVM);
            }

            // Create Unique Name For Image
            // AAEB0DC1-8262-4BAA-B5AD-FED3E1281831.png
            var unqiueFileName = Guid.NewGuid().ToString() + Path.GetExtension(employeeCreateVM.Image!.FileName);

            // Define Path To Save Image
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot",
                "Images",
                "Employees");

            // Create Folder If Not Exist
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Define Full Path To Save Image
            string filePath = Path.Combine(folderPath, unqiueFileName);

            // Write Image To The Defined Path
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                employeeCreateVM.Image.CopyTo(stream);
            }

            var employee = new Employee
            {
                Name = employeeCreateVM.Name,
                Age = employeeCreateVM.Age,
                Salary = employeeCreateVM.Salary,
                ImageUrl = unqiueFileName,
                DepartmentId = employeeCreateVM.DepartmentId
            };

            db.Employees.Add(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        /*------------------------------------------------------------------*/
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var employee = db.Employees.Include(e => e.Department).FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                return RedirectToAction("Index");
            }

            // Map Domain Model To VM
            var employeeEditVM = new EmployeeEditVM
            {
                Id = employee.Id,
                Name = employee.Name,
                Age = employee.Age,
                Salary = employee.Salary,
                DepartmentId = employee.DepartmentId,
                DepartmentName = employee.Department!.Name,
                Departments = GetDepartmentsForDropDown()
            };
            return View(employeeEditVM);
        }
        /*------------------------------------------------------------------*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EmployeeEditVM employeeEditVM)
        {
            var employeeInDb = db.Employees.FirstOrDefault(e => e.Id == employeeEditVM.Id);
            if (employeeInDb == null)
            {
                return RedirectToAction("Index");
            }

            // Map From VM To Domain Model
            employeeInDb.Name = employeeEditVM.Name;
            employeeInDb.Age = employeeEditVM.Age;
            employeeInDb.Salary = employeeEditVM.Salary;
            employeeInDb.DepartmentId = employeeEditVM.DepartmentId;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        /*------------------------------------------------------------------*/
        public IActionResult Delete(int id)
        {
            var employee = db.Employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                return RedirectToAction("Index");
            }
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        /*------------------------------------------------------------------*/
        // Helper Method
        // DRY => Reusable Code => Don't Repeat Yourself
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
