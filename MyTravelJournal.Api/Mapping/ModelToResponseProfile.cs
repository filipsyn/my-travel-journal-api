using AutoMapper;
using MyTravelJournal.Api.Contracts.V1.Responses;
using MyTravelJournal.Api.Models;

namespace MyTravelJournal.Api.Mapping;

public class ModelToResponseProfile: Profile
{
    public ModelToResponseProfile()
    {
        
        CreateMap<User, UserDetailsResponse>();
        CreateMap<Trip, TripDetailsResponse>();
    } 
}