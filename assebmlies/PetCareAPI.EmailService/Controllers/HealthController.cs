using Microsoft.AspNetCore.Mvc;

namespace PetCareAPI.EmailService.Controllers;

[ApiController]
public class HealthController : ControllerBase
{
    [HttpGet("Ping")]
    public IActionResult Get()
    {
        return Ok();
    }
}