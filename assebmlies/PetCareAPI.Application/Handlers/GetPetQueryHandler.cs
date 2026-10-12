using MediatR;
using PetCareAPI.Application.Queries;
using PetCareAPI.Application.Repositories;
using PetCareAPI.Application.Services;
using PetCareAPI.Domain.Models;
using PetCareAPI.Domain.Results;

namespace PetCareAPI.Application.Handlers;

public class GetPetQueryHandler(IPetRepository petRepository, ICurrentUserService userService) : IRequestHandler<GetPetQuery, Result<List<Pet>>>
{
    public async Task<Result<List<Pet>>> Handle(GetPetQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = userService.UserId;

            return userId is null ? Result<List<Pet>>.Failure("User not found") : Result<List<Pet>>.Success(await petRepository.GetAllPets(userId, cancellationToken));
        }
        catch (Exception exception)
        {
            return Result<List<Pet>>.Failure(exception.Message);
        }
        
    }
}