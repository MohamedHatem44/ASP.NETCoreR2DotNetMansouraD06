using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ASP.NETCoreD06.ViewModels.DepartmentEmployees
{
    public class DepartmentsForDropDown
    {
        /*------------------------------------------------------------------*/
        [Required]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        public List<SelectListItem>? Departments { get; set; }
        /*------------------------------------------------------------------*/
    }
}
