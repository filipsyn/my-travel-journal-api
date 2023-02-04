namespace MyTravelJournal.Api.Contracts.V1.Requests;

public record UpdateTripRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime Start { get; set; } = DateTime.Now;
    public DateTime End { get; set; } = DateTime.Now;
    public int UserId { get; set; }
};