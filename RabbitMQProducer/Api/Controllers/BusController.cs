using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusController : ControllerBase
    {
        private readonly IBusService _busService;

        public BusController(IBusService busService)
        {
            _busService = busService;
        }

        [HttpGet, AllowAnonymous]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                return Ok(await _busService.GetAsync());
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> PostAsync([FromQuery] string setValue)
        {            
            try
            {
                await _busService.SetAsync(setValue);
                return Ok();
            }
            catch (Exception e)
            {
                return ExceptionResult(e);
            }
        }

        protected IActionResult ExceptionResult(Exception exception)
        {            
            return StatusCode((int)HttpStatusCode.BadRequest, exception.Message);
        }
    }
}
