using ErrorOr;

namespace MyTravelJournal.Api.Errors;

public static class Trip
{
    public static Error NotFound => Error.NotFound(
        description: "Trip was not found"
    );
}