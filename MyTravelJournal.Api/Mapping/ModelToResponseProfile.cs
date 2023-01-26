using AutoMapper;
using MyTravelJournal.Api.DTOs;
using MyTravelJournal.Api.Models;

namespace MyTravelJournal.Api.Mapping;

public class ModelToResponseProfile: Profile
{
    public ModelToResponseProfile()
    {
        
        CreateMap<User, UserDetailsResponse>();
    } 
}