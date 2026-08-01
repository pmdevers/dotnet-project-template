namespace Template.Api;

public static partial class LoggerExtensions
{
    [LoggerMessage(
        EventId = 1000,
        Level = LogLevel.Information,
        Message = "{Project} services registered")]
    public static partial void ServicesRegistered(this ILogger logger, string project);

    [LoggerMessage(
        EventId = 1001,
        Level = LogLevel.Information,
        Message = "{Project} were configured")]
    public static partial void OptionsConfigured(this ILogger logger, string project);

    [LoggerMessage(
        EventId = 1003,
        Level = LogLevel.Error,
        Message = "Error handling Redis stream entry {EntryId}")]
    public static partial void RedisStreamEntryHandlingError(this ILogger logger, Exception exception, string entryId);

    [LoggerMessage(
        EventId = 1004,
        Level = LogLevel.Debug,
        Message = "Redis consumer group {ConsumerGroup} already exists for stream {StreamName}")]
    public static partial void RedisConsumerGroupAlreadyExists(this ILogger logger, Exception exception, string consumerGroup, string streamName);

    [LoggerMessage(
        EventId = 1005,
        Level = LogLevel.Warning,
        Message = "DROPPING database for fresh start (DatabaseOptions:RecreateOnStartup = true)...")]
    public static partial void DroppingDatabaseForFreshStart(this ILogger logger);

    [LoggerMessage(
        EventId = 1006,
        Level = LogLevel.Information,
        Message = "Database dropped.")]
    public static partial void DatabaseDropped(this ILogger logger);

    [LoggerMessage(
        EventId = 1007,
        Level = LogLevel.Information,
        Message = "Applying pending database migrations...")]
    public static partial void ApplyingPendingDatabaseMigrations(this ILogger logger);

    [LoggerMessage(
        EventId = 1008,
        Level = LogLevel.Information,
        Message = "Database migrations applied successfully.")]
    public static partial void DatabaseMigrationsAppliedSuccessfully(this ILogger logger);

    [LoggerMessage(
        EventId = 1009,
        Level = LogLevel.Error,
        Message = "An error occurred while seeding the database.")]
    public static partial void DatabaseSeedingError(this ILogger logger, Exception exception);
}
