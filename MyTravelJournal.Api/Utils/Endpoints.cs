namespace MyTravelJournal.Api.Utils;

public static class Endpoints
{
    private const string GlobalPrefix = "/api";
    
    public static class User
    {
        private const string ControllerPrefix = $"{GlobalPrefix}/user";
        
        public const string GetUser = $"{ControllerPrefix}";
    }
    
}