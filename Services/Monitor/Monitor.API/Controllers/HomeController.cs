using Microsoft.AspNetCore.Mvc;

namespace Monitor.API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("")]
    public class HomeController : Controller
    {
        [HttpGet, Route("")]
        public ActionResult Index()
        {
            return View();
        }
    }
}