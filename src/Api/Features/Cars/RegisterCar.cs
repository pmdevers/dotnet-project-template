using Microsoft.AspNetCore.Mvc;
using Template.Api.Domain.Abstractions;
using Template.Api.Domain.Entities;
using Template.Api.Domain.ValueObjects;

namespace Template.Api.Features.Cars;

public class Register
{
    public record Command(
        LicensePlate LicensePlate,
        NonEmptyString Name,
        NonEmptyString Description,
        NonEmptyString Brand,
        NonEmptyString Model,
        Category Category,
        Money PricePerDay
        );

    public static async Task<IResult> Handle([FromBody] Command command, IUnitOfWork uow)
    {
        var carRepo = uow.GetRepository<Car, LicensePlate>();
        var car = Car.Create(command.LicensePlate,
            command.Name,
            command.Description,
            command.Brand,
            command.Model,
            command.Category,
            command.PricePerDay);

        carRepo.Add(car);

        await uow.SaveChangesAsync();

        return TypedResults.Created($"/cars/{car.LicensePlate}", car);

    }
}
