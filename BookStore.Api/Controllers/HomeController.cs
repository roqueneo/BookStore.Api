using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Controllers
{
    /// <summary>
    /// Home controller for testing propose
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        /// <summary>
        /// Get method for testing propose
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Hello from Book Store Api");
        }
    }
}
