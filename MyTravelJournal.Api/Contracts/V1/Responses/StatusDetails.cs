namespace MyTravelJournal.Api.Contracts.V1.Responses;

public class StatusDetails
{
    public int Code { get; init; } = StatusCodes.Status200OK;
    public string Message { get; init; } = string.Empty;

}