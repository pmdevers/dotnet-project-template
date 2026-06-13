using Microsoft.AspNetCore.Mvc;
using Template.Api.Domain.Abstractions;
using Template.Api.Domain.Entities;
using Template.Api.Domain.ValueObjects;

namespace Template.Api.Features.Cars;

public static class UpdateSpecifications
{
    public record Command(uint Seats, uint Luggage, Transmission Transmission, FuelType FuelType, uint HorsePower);

    public static async Task<IResult> Handle(
       [FromRoute(Name = "id")] LicensePlate licensePlate,
       [FromBody] Command command,
       [FromServices] IUnitOfWork uow)
    {
        var carRepo = uow.GetRepository<Car, LicensePlate>();
        var car = await carRepo.TryFindAsync(licensePlate);

        if (car is null)
        {
            return TypedResults.NotFound();
        }

        car.SetSpecifications(command.Seats, command.Luggage, command.Transmission, command.FuelType, command.HorsePower);

        await uow.SaveChangesAsync();

        return TypedResults.Ok();
    }
}
