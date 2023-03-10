namespace MyTravelJournal.Api.Contracts.V1;

public static class ApiRoutes
{
    private const string Root = "api";

    private const string Version = "v1";

    private const string Base = $"{Root}/{Version}/";

    public static class User
    {
        public const string ControllerUrl = Base + "users";

        public const string GetAllUsers = "";

        public const string GetUserById = "{id:int}";

        public const string DeleteUser = "{id:int}";

        public const string UpdateUser = "{id:int}";

        public const string GetTripsForUser = "{id:int}/trips";
    }

    public static class Auth
    {
        public const string ControllerUrl = Base + "auth";

        public const string Register = "register";

        public const string Login = "login";
    }

    public static class Trip
    {
        public const string ControllerUrl = Base + "trips";

        public const string GetAll = "";

        public const string GetById = "{id:int}";

        public const string Create = "";

        public const string Update = "{id:int}";

        public const string Delete = "{id:int}";
    }
}