using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode:int}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            return statusCode == 404 ? View("404") : View("Error");
        }

        [Route("Error")]
        public IActionResult Error()
        {
            return View("Error");
        }
    }
}