using Microsoft.EntityFrameworkCore;
using Template.Api.Domain.Abstractions;
using Template.Api.Domain.Queries;
using Template.Api.Domain.ValueObjects;

namespace Template.Api.Infrastructure.Data.Queries;

public class CarQueries(AppDbContext db) : ICarQueries
{
    public async Task<PagedResults<ICarQueries.CarDto>> BrowseCars(int page, int itemsPerPage, CancellationToken ct = default)
    {
        var count = await db.Cars.CountAsync(ct);

        var items = await db.Cars
            .AsNoTracking()
            .Skip((page - 1) * itemsPerPage)
            .Take(itemsPerPage)
            .Select(x => new ICarQueries.CarDto(
                x.LicensePlate,
                x.Name,
                x.Description,
                x.Brand,
                x.Model,
                x.Category.Name,
                x.PricePerDay.ToString(),
                x.Images,
                x.Specifications
            ))
            .ToListAsync(ct);

        return new PagedResults<ICarQueries.CarDto>(items, count, itemsPerPage);
    }

    public async Task<ICarQueries.CarDto?> GetByLicensePlateAsync(LicensePlate licensePlate, CancellationToken ct = default)
        => await db.Cars
            .AsNoTracking()
            .Where(x => x.LicensePlate == licensePlate)
            .Select(x => new ICarQueries.CarDto(x.LicensePlate, x.Name, x.Description, x.Brand, x.Model, x.Category.Name, x.PricePerDay.ToString(), x.Images, x.Specifications!))
            .FirstOrDefaultAsync(ct);
}
