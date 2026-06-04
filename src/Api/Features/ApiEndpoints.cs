using Template.Api.Features.Cars;
using Template.Api.Features.Reservations;

namespace Template.Api.Features;

public static class ApiEndpoints
{
    extension(WebApplication app)
    {

        public WebApplication MapApiEndpoints()
        {
            app.MapCarEndpoints();
            app.MapReservationEndpoints();
            return app;
        }

        public WebApplication MapCarEndpoints()
        {
            var group = app.MapGroup("/cars").WithTags("Cars");

            group.MapPost("/", Register.Handle)
                .WithName("RegisterCar")
                .WithDescription("Registers a new car with the specified license plate.");

            return app;
        }

        public WebApplication MapReservationEndpoints()
        {
            var group = app.MapGroup("/reservations").WithTags("Reservations");
            // Define reservation endpoints here

            group.MapGet("/{id}", GetReservationById.Handle)
                .WithName("GetReservationById")
                .WithDescription("Gets a reservation by its ID.");

            group.MapPost("/", CreateReservation.Handle)
                .WithName("CreateReservation")
                .WithDescription("Creates a new reservation with the specified details.");

            return app;
        }
    }
}
