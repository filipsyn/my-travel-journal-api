namespace MyTravelJournal.Api.Contracts.V1.Responses;

public class ServiceResponse<T>
{
    public T? Data { get; init; } = default(T);
    public bool Success { get; init; }
    public ErrorDetails Error { get; init; } = new();
}