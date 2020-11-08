using BookStore.Api.Contracts;
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
        private readonly ILoggerService _logger;

        public HomeController(ILoggerService logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get method for testing propose
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInfo("Hello from test endpont Book Store Api");
            return Ok("Hello from Book Store Api");
        }
    }
}
