using Microsoft.EntityFrameworkCore;
using Template.Api.Domain.Queries;
using Template.Api.Domain.ValueObjects;

namespace Template.Api.Infrastructure.Data.Queries;

public class CarQueries(AppDbContext db) : ICarQueries
{
    public async Task<ICarQueries.CarDto?> GetByLicensePlateAsync(LicensePlate licensePlate, CancellationToken ct = default)
        => await db.Cars
            .Where(x => x.LicensePlate == licensePlate)
            .Select(x => new ICarQueries.CarDto(x.LicensePlate))
            .FirstOrDefaultAsync(ct);
}
