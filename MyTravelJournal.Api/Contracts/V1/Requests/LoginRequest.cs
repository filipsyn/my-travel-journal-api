namespace MyTravelJournal.Api.Contracts.V1.Requests;

public record LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
};