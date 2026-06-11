using Template.Api.Domain.ValueObjects;

namespace Template.Api.Infrastructure.Data.Models;

public class ReservationModel
{
    public Guid Id { get; set; }
    public LicensePlate LicensePlate { get; set; }
}
