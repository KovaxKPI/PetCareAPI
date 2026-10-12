using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using PetCareAPI.Application.Services;

namespace PetCareAPI.Repository.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService 
{
    public string? UserId =>
        httpContextAccessor.HttpContext?
            .User
            .FindFirst(ClaimTypes.NameIdentifier)?.Value;

    public string? Email =>
        httpContextAccessor.HttpContext?
            .User
            .FindFirst(ClaimTypes.Email)?.Value;
}