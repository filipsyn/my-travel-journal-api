using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace MyTravelJournal.Api.Contracts.V1.Responses;

/// <summary>
/// Contract defining response of services
/// </summary>
/// <typeparam name="T">Type of data returned by service</typeparam>
public class ServiceResponse<T>
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public T? Data { get; init; }

    [JsonIgnore] public bool Success { get; init; }
    public StatusDetails Details { get; init; } = new();


    public ServiceResponse(int code, string message, bool success, T? data = default(T))
    {
        Details.Code = code;
        Details.Message = !message.IsNullOrEmpty() ? message : string.Empty;
        Success = success;
        Data = data;
    }

    public ServiceResponse(int code, string message, T? data = default(T))
    {
        Details.Code = code;
        Details.Message = !message.IsNullOrEmpty() ? message : string.Empty;

        // In this variant of constructor the status is automatically decided from the status code.
        Success = code is >= 100 and < 400;

        Data = data;
    }


    public ServiceResponse()
    {
    }
}