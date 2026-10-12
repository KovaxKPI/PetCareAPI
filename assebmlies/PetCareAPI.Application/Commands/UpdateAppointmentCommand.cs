using MediatR;
using PetCareAPI.Domain.Models;
using PetCareAPI.Domain.Results;

namespace PetCareAPI.Application.Commands;

public record UpdateAppointmentCommand(Guid AppointmentId, DateTimeOffset AppointmentDate, string GooglePlaceId) : IRequest<Result<Appointment>>;