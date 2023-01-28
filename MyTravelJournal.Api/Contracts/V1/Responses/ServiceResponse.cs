using Newtonsoft.Json;

namespace MyTravelJournal.Api.Contracts.V1.Responses;

/// <summary>
/// Contract defining response of services
/// </summary>
/// <typeparam name="T">Type of data returned by service</typeparam>
public class ServiceResponse<T>
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public T? Data { get; init; } = default(T);

    [JsonIgnore] public bool Success { get; init; }
    public StatusDetails Details { get; init; } = new();
}