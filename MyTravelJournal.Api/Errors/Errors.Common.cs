using ErrorOr;

namespace MyTravelJournal.Api.Errors;

public static class Common
{
    public static Error DatabaseConcurrencyError => Error.Conflict(
        code: "Database.ConcurrencyConflict",
        description: "Database concurrency error"
    );

    public static Error FaultyMapping => Error.Failure(
        description: "Unsuccessful mapping of objects"
    );
}