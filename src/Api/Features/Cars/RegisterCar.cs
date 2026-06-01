using Microsoft.AspNetCore.Mvc;
using Template.Api.Domain.Abstractions;
using Template.Api.Domain.Entities;
using Template.Api.Domain.ValueObjects;

namespace Template.Api.Features.Cars;

public class Register
{
    public record Command(LicensePlate LicensePlate);

    public static async Task<IResult> Handle([FromBody] Command command, IUnitOfWork uow)
    {
        var carRepo = uow.GetRepository<Car, CarId>();
        var car = Car.Create(command.LicensePlate);

        carRepo.Add(car);

        await uow.SaveChangesAsync();

        return TypedResults.Created($"/cars/{car.Id}", car);

    }
}
