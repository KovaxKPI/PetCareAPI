using Azure.Storage.Blobs;
using MediatR;
using PetCareAPI.Application.Commands;
using PetCareAPI.Application.Repositories;
using PetCareAPI.Application.Services;
using PetCareAPI.Domain.Models;
using PetCareAPI.Domain.Results;

namespace PetCareAPI.Application.Handlers.Commands;

public class AddPetCommandHandler(BlobServiceClient blobServiceClient, IPetRepository petRepository, ICurrentUserService userService) : IRequestHandler<AddPetCommand, Result<Guid>>
{
    private const string ContainerName = "test";

    public async Task<Result<Guid>> Handle(AddPetCommand request, CancellationToken cancellationToken)
    {
        var blobName = $"{Guid.NewGuid()}/{request.File.FileName}";
        try
        {
            var userId = userService.UserId;

            if (userId is null)
                return Result<Guid>.Failure("User not found");
            
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(ContainerName);
            var blobClient = blobContainerClient.GetBlobClient(blobName);
            await using var content = request.File.OpenReadStream();
            var response = await blobClient.UploadAsync(content, cancellationToken);

            if (!response.HasValue)
            {
                return Result<Guid>.Failure("Failed to upload file");
            }
        
            var pet = new Pet {Name = request.Name, Gender = request.Gender, Age = request.Age, ImageVersionId = blobName, UserId = userId};
        
            await petRepository.AddPetAsync(pet, cancellationToken);
            
            return Result<Guid>.Success(pet.PetId);
        }
        catch (Exception exception)
        {
            return Result<Guid>.Failure(exception);
        }
        
    }
}