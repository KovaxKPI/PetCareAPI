using MediatR;
using PetCareAPI.Domain.Results;

namespace PetCareAPI.Application.Commands;

public record DeleteAppointmentCommand(Guid AppointmentId) : IRequest<Result>;