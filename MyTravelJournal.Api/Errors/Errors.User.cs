using ErrorOr;

namespace MyTravelJournal.Api.Errors;

public static class User
{
    public static Error NotFound => Error.NotFound(
        code: "User.NotFound",
        description: "User was not found"
    );

}