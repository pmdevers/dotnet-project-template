using Template.Api.Domain.Abstractions;
using Template.Api.Domain.Entities;
using Template.Api.Domain.ValueObjects;

namespace Template.Api.Domain.Queries;

public interface ICarQueries
{
    public record CarDto(
        LicensePlate LicensePlate,
        string Name,
        string Description,
        string Brand,
        string Model,
        string Category,
        string PricePerDay,
        string[] Images,
        Specifications Specifications
        );

    public record CarSpecs(int Seats, int Luggage, string Transmission, string FuelType, int HorsePower);
    public record Availablity(DateTime[] BlockedDates);

    public Task<CarDto?> GetByLicensePlateAsync(LicensePlate licensePlate, CancellationToken ct = default);

    public Task<PagedResults<CarDto>> BrowseCars(int page, int itemsPerPage, CancellationToken ct = default);
}
