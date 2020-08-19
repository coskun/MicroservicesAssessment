using Microsoft.AspNetCore.Mvc;

namespace Consumer.API.Controllers
{
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet, Route("")]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}