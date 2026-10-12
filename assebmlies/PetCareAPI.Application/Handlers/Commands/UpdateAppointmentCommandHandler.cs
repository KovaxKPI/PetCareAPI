using MediatR;
using PetCareAPI.Application.Commands;
using PetCareAPI.Application.Repositories;
using PetCareAPI.Application.Services;
using PetCareAPI.Contracts.Events;
using PetCareAPI.Domain.Models;
using PetCareAPI.Domain.Results;

namespace PetCareAPI.Application.Handlers.Commands;

public class UpdateAppointmentCommandHandler(IPetRepository petRepository, ICurrentUserService userService) : IRequestHandler<UpdateAppointmentCommand, Result<Appointment>>
{
    public async Task<Result<Appointment>> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userEmail = userService.Email;

            if (userEmail == null)
                return Result<Appointment>.Failure("No user email provided");
            
            var appointment = await petRepository.GetAppointmentById(request.AppointmentId, cancellationToken);

            if (appointment is null)
                throw new KeyNotFoundException("Appointment not found");
            
            var pet = await petRepository.GetPetById(appointment.PetId, cancellationToken);
            
            if (pet is null)
                throw new KeyNotFoundException("Pet not found");
            
            appointment.Update(request.AppointmentDate, request.GooglePlaceId);
            
            await petRepository.UpdateAppointmentAsync(new AppointmentUpdatedEvent(appointment.AppointmentId, pet.Name, appointment.AppointmentDate, appointment.GooglePlaceId, userEmail),
                cancellationToken);
            
            return Result<Appointment>.Success(appointment);
        }
        catch (Exception exception)
        {
            return Result<Appointment>.Failure(exception);
        }
    }
}