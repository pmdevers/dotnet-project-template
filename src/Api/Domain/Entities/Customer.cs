using Template.Api.Domain.Abstractions;
using Template.Api.Domain.ValueObjects;

namespace Template.Api.Domain.Entities;

[GenerateId]
public class Customer : AggregateRoot
{
    public CustomerId Id { get; private set; }
    public NonEmptyString Name { get; private set; }

    public static Customer Create(NonEmptyString name)
    {
        var customer = new Customer()
        {
            Id = CustomerId.New(),
            Name = name,
        };

        return customer;
    }
}
