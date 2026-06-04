using Template.Api.Domain.Abstractions;

namespace Template.Api.Domain.Entities;

[GenerateId]
public class Customer : AggregateRoot
{
    public CustomerId Id { get; private set; }
    public string Name { get; private init; } = string.Empty;

    public static Customer Create(string name)
    {
        var customer = new Customer()
        {
            Id = CustomerId.New(),
            Name = name,
        };

        return customer;
    }
}
