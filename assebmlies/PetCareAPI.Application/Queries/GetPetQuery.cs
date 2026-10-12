using MediatR;
using PetCareAPI.Domain.Models;
using PetCareAPI.Domain.Results;

namespace PetCareAPI.Application.Queries;

public record GetPetQuery : IRequest<Result<List<Pet>>>;