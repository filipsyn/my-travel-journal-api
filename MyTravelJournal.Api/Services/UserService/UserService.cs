using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using MyTravelJournal.Api.Contracts.V1.Requests;
using MyTravelJournal.Api.Contracts.V1.Responses;
using MyTravelJournal.Api.Models;
using MyTravelJournal.Api.Repositories.UserRepository;
using MyTravelJournal.Api.Services.TripService;
using MyTravelJournal.Api.Exceptions;

namespace MyTravelJournal.Api.Services.UserService;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly ITripService _tripService;
    private readonly IUserRepository _userRepository;

    public UserService(IMapper mapper, ITripService tripService, IUserRepository userRepository)
    {
        _mapper = mapper;
        _tripService = tripService;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserDetailsResponse>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();

        return _mapper.Map<IEnumerable<UserDetailsResponse>>(users);
    }


    public async Task<UserDetailsResponse> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
            throw new NotFoundException("User was not found");

        return _mapper.Map<UserDetailsResponse>(user);
    }

    public async Task<UserDetailsResponse> GetByUsernameAsync(string username)
    {
        var user = await _userRepository.GetByUsernameAsync(username);

        if (user is null)
            throw new NotFoundException("User was not found");

        return _mapper.Map<UserDetailsResponse>(user);
    }

    public async Task CreateAsync(CreateUserRequest request)
    {
        var user = _mapper.Map<User>(request);

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        try
        {
            await _userRepository.CreateAsync(user);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ConflictException("Conflict occured while adding to database");
        }
    }


    public async Task UpdateAsync(
        JsonPatchDocument<UpdateUserDetailsRequest> patchRequest, int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
            throw new NotFoundException("User was not found");

        var patchedUser = _mapper.Map<JsonPatchDocument<User>>(patchRequest);

        if (patchedUser is null)
            throw new Exception("Error occured while mapping objects");

        patchedUser.ApplyTo(user);

        try
        {
            await _userRepository.UpdateAsync(user);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ConflictException("Conflict occured while adding to database");
        }

    }


    public async Task DeleteByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
            throw new NotFoundException("User was not found");

        try
        {
            await _userRepository.DeleteAsync(id);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ConflictException("Conflict occured while adding to database");
        }
    }

    public async Task<IEnumerable<TripDetailsResponse>> GetTripsForUser(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
            throw new NotFoundException("User was not found");

        var response = await _tripService.GetTripsByUser(id);
        return response.ToList();
    }
}