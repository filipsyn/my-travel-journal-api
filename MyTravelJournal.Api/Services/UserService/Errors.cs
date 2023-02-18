using ErrorOr;

namespace MyTravelJournal.Api.Services.UserService;

public static class Errors
{
    public static class User
    {
        public static Error NotFound => Error.NotFound(
            code: "User.NotFound",
            description: "User was not found"
        );
    }
}