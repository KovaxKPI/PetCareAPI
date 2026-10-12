using MediatR;
using Microsoft.AspNetCore.Http;
using PetCareAPI.Domain.Results;

namespace PetCareAPI.Application.Commands;

public record AddPetCommand(string Name, int Age, string Gender, IFormFile File) : IRequest<Result<Guid>>;