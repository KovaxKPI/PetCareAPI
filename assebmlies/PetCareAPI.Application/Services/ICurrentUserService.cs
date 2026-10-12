namespace PetCareAPI.Application.Services;

public interface ICurrentUserService
{
    string? UserId { get; }
    
    string? Email { get; }
}