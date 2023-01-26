using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using MyTravelJournal.Api.DTOs;
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
    }
}