using Microsoft.AspNetCore.Mvc;
namespace WebApp_Challenge.Controllers
{
    
        [ApiController]
        [Route("[controller]")]
        public class HealthController : ControllerBase
        {
            [HttpGet]
            public IActionResult Get()
            {
                return Ok("API is running correctly");
            }
        }
    
}
