namespace MyTravelJournal.Api.DTOs;

public record UserDetailsDto
{
    public int UserId { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;

    public override string ToString()
    {
        return $"{Username} ({FirstName} {LastName}) ID: {UserId}";
    }
}