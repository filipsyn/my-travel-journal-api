namespace MyTravelJournal.Api.Contracts.V1.Responses;

public class ServiceResponse<T>
{
    public T? Data { get; set; }
    public bool Success { get; set; }
    public ErrorDetails Error { get; set; } = new();
}