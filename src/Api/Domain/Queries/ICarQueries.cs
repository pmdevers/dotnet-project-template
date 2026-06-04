using Template.Api.Domain.ValueObjects;

namespace Template.Api.Domain.Queries;

public interface ICarQueries
{
    public record struct CarDto(Guid Id, LicensePlate LicensePlate);
    public Task<CarDto?> GetByLicensePlateAsync(LicensePlate licensePlate, CancellationToken ct = default);
}
