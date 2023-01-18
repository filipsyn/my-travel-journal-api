namespace MyTravelJournal.Api.Utils;

public static class Endpoints
{
    public static class User
    {
        public const string ControllerUrl = "/api/user";

        public const string GetAllUsers = "";

        public const string GetUserById = "{id:int}";

        public const string CreateUser = "";

        public const string DeleteUser = "{id:int}";

        public const string UpdateUser = "{id:int}";
    }
}