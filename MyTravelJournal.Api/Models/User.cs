namespace MyTravelJournal.Api.Models;

public class User
{
    public int UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = "User";
    public string PasswordHash { get; set; } = string.Empty;

    public List<Trip>? Trips { get; set; }
}