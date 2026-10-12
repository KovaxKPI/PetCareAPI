using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCareAPI.Application.Commands;
using PetCareAPI.Application.Queries;

namespace PetCareAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PetController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllUserPets(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetPetQuery(), cancellationToken);
        
        return result.IsSuccess ? Ok(result.Value) : StatusCode(500, result.Message);
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddPet([FromForm] AddPetCommand request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request, cancellationToken);
        
        return result.IsSuccess ? Ok(result.Value) : StatusCode(500, result.Message);
    }
    
    [HttpPost("Appointment")]
    [Authorize]
    public async Task<IActionResult> AddAppointment([FromBody] AddAppointmentCommand request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request, cancellationToken);
        
        return result.IsSuccess ? Ok(result.Value) : StatusCode(500, result.Message);
    }
    
    [HttpDelete("Appointment")]
    [Authorize]
    public async Task<IActionResult> DeleteAppointment([FromQuery] DeleteAppointmentCommand request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request, cancellationToken);
        
        return result.IsSuccess ? Ok() : StatusCode(500, result.Message);
    }
    
    [HttpPut("Appointment/Update")]
    [Authorize]
    public async Task<IActionResult> UpdateAppointment([FromBody] UpdateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request, cancellationToken);
        
        return result.IsSuccess ? Ok(result.Value) : StatusCode(500, result.Message);
    }
}