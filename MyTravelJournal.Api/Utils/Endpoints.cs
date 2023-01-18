namespace MyTravelJournal.Api.Utils;

public static class Endpoints
{
    public static class User
    {
        public const string GetAllUsers = "/api/user";

        public const string GetUser = "/api/user/{id}";

        public const string CreateUser = "/api/user/";

        public const string DeleteUser = "/api/user/{id}";

        public const string UpdateUser = "/api/user/{id}";
    }
}