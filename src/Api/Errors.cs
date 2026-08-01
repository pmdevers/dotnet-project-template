using System.Text.Json;

namespace Template.Api;

public static class Errors
{
    public static InvalidOperationException DefaultConnectionIsNotSet()
        => new("DefaultConnection is not set in the configuration.");

    public static InvalidOperationException PostgresConnectionStringEnvironmentVariableIsNotSet()
        => new("POSTGRES_CONNECTION_STRING environment variable is not set.");

    public static InvalidOperationException AggregateRootMustHaveValidId()
        => new("Aggregate root must have a valid Id.");

    public static InvalidOperationException EntityNotFound()
        => new("Entity not found");

    public static InvalidOperationException NoDomainEventHandlersMatchingSignature(string handlerTypeName)
        => new($"{handlerTypeName} has no methods matching the DomainEventHandler delegate signature 'Task MethodName(T message, CancellationToken ct) where T : DomainEvent'.");

    public static ArgumentException ReservationDateCannotBeInThePast()
        => new("Reservation date cannot be in the past.");

    public static ArgumentNullException CurrencyCodeCannotBeNullOrWhiteSpace(string paramName)
        => new(paramName);

    public static ArgumentException SuccessfulResultCannotHaveError(string paramName)
        => new("Successful result cannot have an error.", paramName);

    public static ArgumentException FailedResultCannotHaveValue(string paramName)
        => new("Failed result cannot have a value.", paramName);

    public static ArgumentOutOfRangeException AmountCannotBeNegative(string paramName)
        => new(paramName, "Amount cannot be negative.");

    public static ArgumentException ValueCannotBeEmpty(string paramName)
        => new("Value cannot be empty.", paramName);

    public static ArgumentException LicensePlateCannotBeEmpty()
        => new("LicensePlate cannot be empty.");

    public static ArgumentException MoneyCurrencyCannotBeEmpty(string paramName)
        => new("Currency cannot be empty.", paramName);

    public static ArgumentOutOfRangeException TotalCountCannotBeNegative(string paramName)
        => new(paramName, "Total count cannot be negative.");

    public static JsonException CannotConvertNullToType(string typeName)
        => new($"Cannot convert null to {typeName}.");

    public static JsonException UnsupportedTokenTypeForType(JsonTokenType tokenType, string typeName)
        => new($"Token type '{tokenType}' is not supported for {typeName}.");

    public static JsonException InvalidValueObjectValue(string typeName, string? value)
        => new($"Invalid {typeName} value '{value}'.");
}
