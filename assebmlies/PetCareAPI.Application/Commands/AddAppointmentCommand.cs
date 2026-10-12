using MediatR;
using PetCareAPI.Domain.Results;

namespace PetCareAPI.Application.Commands;

public record AddAppointmentCommand(Guid PetId, DateTimeOffset AppointmentDate, string GooglePlaceId) : IRequest<Result<Guid>>;