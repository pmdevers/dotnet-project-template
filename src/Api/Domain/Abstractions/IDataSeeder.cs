namespace Template.Api.Domain.Abstractions;

public interface IDataSeeder
{
    Task SeedAsync(bool recreate, CancellationToken cancellationToken = default);
}
