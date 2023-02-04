using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using MyTravelJournal.Api.Contracts.V1.Requests;
using MyTravelJournal.Api.Models;

namespace MyTravelJournal.Api.Mapping;

public class RequestToModelProfile : Profile
{
    public RequestToModelProfile()
    {
        CreateMap<CreateUserRequest, User>();
        CreateMap<UpdateUserDetailsRequest, User>();
        CreateMap<JsonPatchDocument<UpdateUserDetailsRequest>, JsonPatchDocument<User>>();
        CreateMap<Operation<UpdateUserDetailsRequest>, Operation<User>>();

        CreateMap<CreateTripRequest, Trip>();
        CreateMap<JsonPatchDocument<UpdateTripRequest>, JsonPatchDocument<Trip>>();
        CreateMap<Operation<UpdateTripRequest>, Operation<Trip>>();
    }
}