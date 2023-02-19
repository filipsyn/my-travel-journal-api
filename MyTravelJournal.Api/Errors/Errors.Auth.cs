using ErrorOr;

namespace MyTravelJournal.Api.Errors;

public static class Auth
{
    public static Error UsernameTaken => Error.Conflict(
        description: "User with this username already exists"
    );

    public static Error IncorrectCredentials => Error.Validation(
        description: "Incorrect credentials"
    );
}