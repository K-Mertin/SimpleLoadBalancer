using Microsoft.AspNetCore.Mvc;

namespace LoadBalancer.Controllers
{
    public class ApiController : Controller
    {
        //only for swagger use
        [HttpGet("api")]
        public IActionResult ConvertFile(string uri)
        {
            return Ok();
        }
    }
}