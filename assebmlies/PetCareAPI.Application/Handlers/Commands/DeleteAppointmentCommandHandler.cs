using MediatR;
using PetCareAPI.Application.Commands;
using PetCareAPI.Application.Repositories;
using PetCareAPI.Application.Services;
using PetCareAPI.Contracts.Events;
using PetCareAPI.Domain.Results;

namespace PetCareAPI.Application.Handlers.Commands;

public class DeleteAppointmentCommandHandler(IPetRepository petRepository, ICurrentUserService userService) : IRequestHandler<DeleteAppointmentCommand, Result>
{
    public async Task<Result> Handle(DeleteAppointmentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userEmail = userService.Email;

            if (userEmail == null)
                return Result.Failure("No user email provided");
            
            var appointment = await petRepository.GetAppointmentById(request.AppointmentId, cancellationToken);

            if (appointment is null)
                throw new KeyNotFoundException("Appointment not found");
            
            var pet = await petRepository.GetPetById(appointment.PetId, cancellationToken);
            
            if (pet is null)
                throw new KeyNotFoundException("Pet not found");
            
            await petRepository.DeleteAppointmentAsync(appointment,
                new AppointmentDeletedEvent(appointment.AppointmentId, pet.Name, appointment.AppointmentDate, userEmail),
                cancellationToken);
            return Result.Success();
        }
        catch (Exception exception)
        {
            return Result.Failure(exception);
        }
    }
}