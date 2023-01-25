using AutoMapper;
using MyTravelJournal.Api.DTOs;
using MyTravelJournal.Api.Models;

namespace MyTravelJournal.Api.Utils;

public class AutoMapperProfile: Profile
{
   public AutoMapperProfile()
   {
      CreateMap<UserDetailsDto, User>();
   }
}