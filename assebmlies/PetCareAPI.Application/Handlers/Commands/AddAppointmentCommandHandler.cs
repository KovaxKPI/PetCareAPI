using MediatR;
using PetCareAPI.Application.Commands;
using PetCareAPI.Application.Repositories;
using PetCareAPI.Application.Services;
using PetCareAPI.Contracts.Events;
using PetCareAPI.Domain.Models;
using PetCareAPI.Domain.Results;

namespace PetCareAPI.Application.Handlers.Commands;

public class AddAppointmentCommandHandler(IPetRepository petRepository, ICurrentUserService userService) : IRequestHandler<AddAppointmentCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(AddAppointmentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userEmail = userService.Email;

            if (userEmail == null)
                return Result<Guid>.Failure("No user email provided");
            
            var pet = await petRepository.GetPetById(request.PetId, cancellationToken);
            
            if (pet is null)
                throw new KeyNotFoundException("Pet not found");
            
            var appointment = new Appointment {PetId = request.PetId, AppointmentDate = request.AppointmentDate, GooglePlaceId = request.GooglePlaceId};
            
            await petRepository.AddAppointmentAsync(appointment, 
                new AppointmentCreatedEvent(appointment.AppointmentId, pet.Name,appointment.AppointmentDate, appointment.GooglePlaceId, userEmail),
                cancellationToken);
            
            return Result<Guid>.Success(appointment.AppointmentId);
        }
        catch (Exception exception)
        {
            return Result<Guid>.Failure(exception);
        }
    }
}