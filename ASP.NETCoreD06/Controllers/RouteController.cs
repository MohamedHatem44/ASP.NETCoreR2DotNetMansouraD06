using Microsoft.AspNetCore.Mvc;

namespace ASP.NETCoreD06.Controllers
{
    public class RouteController : Controller
    {
        /*------------------------------------------------------------------*/
        // Get: ~/Route/Index/1/John
        public IActionResult Index(string name, int id)
        {
            return Content($"Name: {name}, ID: {id}");
        }
        /*------------------------------------------------------------------*/
        public IActionResult About(int id)
        {
            return Content($"ID: {id}");
        }
        /*------------------------------------------------------------------*/
        [HttpGet("/Test")]
        public IActionResult About2()
        {
            return Content("About 2");
        }
        /*------------------------------------------------------------------*/
    }
}
