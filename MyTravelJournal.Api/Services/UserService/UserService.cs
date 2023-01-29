using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using MyTravelJournal.Api.Contracts.V1.Requests;
using MyTravelJournal.Api.Contracts.V1.Responses;
using MyTravelJournal.Api.Data;
using MyTravelJournal.Api.Models;

namespace MyTravelJournal.Api.Services.UserService;

public class UserService : IUserService
{
    private readonly DataContext _db;
    private readonly IMapper _mapper;

    public UserService(DataContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<ServiceResponse<IEnumerable<UserDetailsResponse>>> GetAllAsync()
    {
        var users = await _db.Users.ToListAsync();
        return new ServiceResponse<IEnumerable<UserDetailsResponse>>
        {
            Success = true,
            Data = _mapper.Map<IEnumerable<UserDetailsResponse>>(users),
            Details = new StatusDetails
            {
                Code = StatusCodes.Status200OK,
                Message = "List of users successfully retrieved."
            }
        };
    }


    public async Task<ServiceResponse<UserDetailsResponse>> GetByIdAsync(int id)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == id);

        if (user is null)
        {
            return new ServiceResponse<UserDetailsResponse>
            {
                Success = false,
                Details = new StatusDetails
                {
                    Code = StatusCodes.Status404NotFound,
                    Message = "User with this ID was not found."
                }
            };
        }

        return new ServiceResponse<UserDetailsResponse>
        {
            Success = true,
            Data = _mapper.Map<UserDetailsResponse>(user),
            Details = new StatusDetails
            {
                Code = StatusCodes.Status200OK,
                Message = "User successfully retrieved"
            }
        };
    }

    public async Task<ServiceResponse<UserDetailsResponse>> GetByUsernameAsync(string username)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user is null)
        {
            return new ServiceResponse<UserDetailsResponse>
            {
                Success = false,
                Details = new StatusDetails
                {
                    Code = StatusCodes.Status404NotFound,
                    Message = "User with this username doesn't exist."
                }
            };
        }

        return new ServiceResponse<UserDetailsResponse>
        {
            Success = true,
            Data = _mapper.Map<UserDetailsResponse>(user),
            Details = new StatusDetails
            {
                Code = StatusCodes.Status200OK,
                Message = "User successfully found."
            }
        };
    }

    public async Task<ServiceResponse<string>> CreateAsync(CreateUserRequest request)
    {
        var user = _mapper.Map<User>(request);
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        
        _db.Users.Add(user);

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            return new ServiceResponse<string>
            {
                Success = false,
                Details = new StatusDetails
                {
                    Code = StatusCodes.Status409Conflict,
                    Message = ex.ToString(),
                }
            };
        }

        return new ServiceResponse<string>
        {
            Success = true,
            Details = new StatusDetails
            {
                Code = StatusCodes.Status200OK,
                Message = "User was successfully created."
            }
        };
    }


    public async Task<ServiceResponse<UserDetailsResponse>> UpdateAsync(
        JsonPatchDocument<UpdateUserDetailsRequest> patchRequest, int id)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == id);
        if (user is null)
        {
            return new ServiceResponse<UserDetailsResponse>
            {
                Success = false,
                Details = new StatusDetails
                {
                    Code = StatusCodes.Status404NotFound,
                    Message = "User with this ID was not found."
                }
            };
        }

        var patchedUser = _mapper.Map<JsonPatchDocument<User>>(patchRequest);
        if (patchedUser is null)
        {
            return new ServiceResponse<UserDetailsResponse>
            {
                Success = false,
                Details = new StatusDetails
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Message = "Mapping of objects was unsuccessful."
                }
            };
        }

        patchedUser.ApplyTo(user);

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            return new ServiceResponse<UserDetailsResponse>
            {
                Success = false,
                Details = new StatusDetails
                {
                    Code = StatusCodes.Status409Conflict,
                    Message = ex.ToString()
                }
            };
        }


        return new ServiceResponse<UserDetailsResponse>
        {
            Success = true,
            Details = new StatusDetails
            {
                Code = StatusCodes.Status200OK,
                Message = "User was successfully updated."
            }
        };
    }


    public async Task<ServiceResponse<UserDetailsResponse>> DeleteByIdAsync(int id)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == id);
        if (user is null)
        {
            return new ServiceResponse<UserDetailsResponse>
            {
                Success = false,
                Details = new StatusDetails
                {
                    Code = StatusCodes.Status404NotFound,
                    Message = "User with this ID was not found."
                }
            };
        }

        _db.Users.Remove(user);

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            return new ServiceResponse<UserDetailsResponse>
            {
                Success = false,
                Details = new StatusDetails
                {
                    Code = StatusCodes.Status409Conflict,
                    Message = ex.ToString()
                }
            };
        }

        return new ServiceResponse<UserDetailsResponse>
        {
            Success = true,
            Details = new StatusDetails
            {
                Code = StatusCodes.Status200OK,
                Message = "User was successfully deleted."
            }
        };
    }
}