using Microsoft.AspNetCore.Mvc;

namespace Monitor.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("")]
    public class HomeController : Controller
    {
        [HttpGet, Route("")]
        public IActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}