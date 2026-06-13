using Microsoft.AspNetCore.Mvc;
using Template.Api.Domain.Queries;

namespace Template.Api.Features.Cars;

public static class BrowseCars
{
    public static async Task<IResult> Handle(
        ICarQueries queries,
        [FromQuery(Name = "page")] int page = 1,
        [FromQuery(Name = "itemsPerPage")] int itemsPerPage = 10,
        CancellationToken ct = default
        )
    {
        var result = await queries.BrowseCars(page, itemsPerPage, ct);

        return Results.Ok(result);
    }
}
