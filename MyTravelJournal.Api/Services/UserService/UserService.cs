using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using MyTravelJournal.Api.Contracts.V1.Requests;
using MyTravelJournal.Api.Contracts.V1.Responses;
using MyTravelJournal.Api.Models;
using MyTravelJournal.Api.Repositories.UserRepository;
using MyTravelJournal.Api.Services.TripService;
using ErrorOr;

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


    // public async Task<ServiceResponse<UserDetailsResponse>> GetByIdAsync(int id)
    public async Task<ErrorOr<UserDetailsResponse>> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
            return Errors.User.NotFound;

        return _mapper.Map<UserDetailsResponse>(user);
    }

    public async Task<ErrorOr<UserDetailsResponse>> GetByUsernameAsync(string username)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        
        if (user is null)
            return Errors.User.NotFound;

        return _mapper.Map<UserDetailsResponse>(user);
    }

    public async Task<ErrorOr<Created>> CreateAsync(CreateUserRequest request)
    {
        var user = _mapper.Map<User>(request);

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        try
        {
            await _userRepository.CreateAsync(user);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Error.Conflict(description: "Database concurrency exception");
        }

        return Result.Created;
    }


    public async Task<ServiceResponse<UserDetailsResponse>> UpdateAsync(
        JsonPatchDocument<UpdateUserDetailsRequest> patchRequest, int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user is null)
        {
            return new ServiceResponse<UserDetailsResponse>(
                StatusCodes.Status404NotFound,
                "User with this ID was not found."
            );
        }

        var patchedUser = _mapper.Map<JsonPatchDocument<User>>(patchRequest);
        if (patchedUser is null)
        {
            return new ServiceResponse<UserDetailsResponse>(
                StatusCodes.Status500InternalServerError,
                "Mapping of objects was unsuccessful."
            );
        }

        patchedUser.ApplyTo(user);

        try
        {
            await _userRepository.UpdateAsync(user);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            return new ServiceResponse<UserDetailsResponse>(
                StatusCodes.Status409Conflict,
                ex.ToString()
            );
        }

        return new ServiceResponse<UserDetailsResponse>(
            StatusCodes.Status200OK,
            "User was successfully updated."
        );
    }


    public async Task<ErrorOr<Deleted>> DeleteByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
            return Errors.User.NotFound;

        try
        {
            await _userRepository.DeleteAsync(id);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Errors.User.DatabaseConcurrencyError;
        }

        return Result.Deleted;
    }

    public async Task<ServiceResponse<IEnumerable<TripDetailsResponse>>> GetTripsForUser(int id)
    {
        /*
        var user = await this.GetByIdAsync(id);
        if (!user.Success)
        {
            return new ServiceResponse<IEnumerable<TripDetailsResponse>>(
                StatusCodes.Status404NotFound,
                $"User with ID {id} was not found."
            );
        }

        var tripsResponse = await _tripService.GetTripsByUser(id);

        return new ServiceResponse<IEnumerable<TripDetailsResponse>>(
            StatusCodes.Status200OK,
            $"Trips for user with ID {id} were successfully retrieved.",
            tripsResponse.Data
        );
        */
        return new ServiceResponse<IEnumerable<TripDetailsResponse>>(200, "ok");
    }
}