namespace MyTravelJournal.Api.Contracts.V1.Responses;

public record UserDetailsResponse
{
    public int UserId { get; init; }
    public string Username { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
};