using AutoMapper;
using MyTravelJournal.Api.DTOs;
using MyTravelJournal.Api.Models;

namespace MyTravelJournal.Api.Mapping;

public class RequestToModelProfile : Profile
{
    public RequestToModelProfile()
    {
        CreateMap<UserDetailsDto, User>();
    }
}