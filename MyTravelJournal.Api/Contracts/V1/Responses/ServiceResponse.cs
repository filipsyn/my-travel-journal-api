namespace MyTravelJournal.Api.Contracts.V1.Responses;

/// <summary>
/// Contract defining response of services
/// </summary>
/// <typeparam name="T">Type of data returned by service</typeparam>
public class ServiceResponse<T>
{
    public T? Data { get; init; } = default(T);
    public bool Success { get; init; }
    public ErrorDetails Error { get; init; } = new();
}