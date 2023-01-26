namespace MyTravelJournal.Api.Utils;

public static class Endpoints
{
    private const string Root = "api";

    private const string Version = "v1";

    private const string Base = $"{Root}/{Version}/";

    public static class User
    {
        public const string ControllerUrl = Base + "users";

        public const string GetAllUsers = "";

        public const string GetUserById = "{id:int}";

        public const string CreateUser = "";

        public const string DeleteUser = "{id:int}";

        public const string UpdateUser = "{id:int}";
    }
}