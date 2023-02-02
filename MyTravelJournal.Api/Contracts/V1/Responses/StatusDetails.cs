namespace MyTravelJournal.Api.Contracts.V1.Responses;

public class StatusDetails
{
    public int Code { get; set; } = StatusCodes.Status200OK;
    public string Message { get; set; } = string.Empty;

}