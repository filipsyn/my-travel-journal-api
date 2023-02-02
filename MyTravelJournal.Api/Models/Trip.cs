namespace MyTravelJournal.Api.Models;

public class Trip
{
    public int TripId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime Start { get; set; } = DateTime.Now;
    public DateTime End { get; set; } = DateTime.Now;

    public int UserId { get; set; }
    public User? User { get; set; }
}