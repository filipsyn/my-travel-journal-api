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

    /// <summary>
    /// Asynchronously retrieves list of all users.
    /// </summary>
    /// <returns>
    /// <para>
    /// A standardized response body <see cref="ServiceResponse{T}"/> carrying data with type of
    /// <see cref="IEnumerable{T}"/> containing <see cref="UserDetailsResponse"/>.
    /// </para>
    /// </returns>
    /// 
    /// <remarks>
    /// This method returns multiple variants of <see cref="ServiceResponse{T}"/> with different contents.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Status</term>
    ///         <description>Response payload</description>
    ///     </listheader>
    ///     <item>
    ///         <term><c>200</c></term>
    ///         <description>Returns <c>data</c> payload, <c>OK</c> status code and <c>Success</c> set to <c>true</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
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


    /// <summary>
    /// Asynchronously retrieves data about specific user.
    /// </summary>
    /// <param name="id">A unique ID of searched user, which is to be retrieved</param>
    /// <returns>
    /// <para>
    /// A standardized response body <see cref="ServiceResponse{T}"/> carrying data with type of <see cref="UserDetailsResponse"/>.
    /// </para>
    /// </returns>
    /// 
    /// <remarks>
    /// This method returns multiple variants of <see cref="ServiceResponse{T}"/> with different contents.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Status</term>
    ///         <description>Response payload</description>
    ///     </listheader>
    ///     <item>
    ///         <term><c>200</c></term>
    ///         <description>Returns <c>data</c> payload, <c>OK</c> status and <c>Success</c> set to <c>true</c></description>
    ///     </item>
    /// 
    ///     <item>
    ///         <term><c>404</c></term>
    ///         <description>Returns <c>NOT FOUND</c> status and <c>Success</c> set to <c>false</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
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


    /// <summary>
    /// Asynchronously creates new user, using information passed in a parameter. 
    /// </summary>
    /// <param name="request">A request body with information about new user</param>
    /// <returns>
    /// <para>
    /// A standardized response body <see cref="ServiceResponse{T}"/> carrying data with type of <see cref="UserDetailsResponse"/>.
    /// </para>
    /// </returns>
    /// 
    /// <remarks>
    /// This method returns multiple variants of <see cref="ServiceResponse{T}"/> with different contents.
    /// <list type="table">
    ///     <listheader>
    ///         <term>Status</term>
    ///         <description>Response payload</description>
    ///     </listheader>
    ///     <item>
    ///         <term><c>200</c></term>
    ///         <description>Returns <c>OK</c> status and <c>Success</c> set to <c>true</c></description>
    ///     </item>
    /// 
    ///     <item>
    ///         <term><c>409</c></term>
    ///         <description>Returns <c>CONFLICT</c> status and <c>Success</c> set to <c>false</c></description>
    ///     </item>
    /// </list>
    /// </remarks>
    public async Task<ServiceResponse<UserDetailsResponse>> CreateAsync(CreateUserRequest request)
    {
        var user = _mapper.Map<User>(request);
        _db.Users.Add(user);

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
                    Message = ex.ToString(),
                }
            };
        }

        return new ServiceResponse<UserDetailsResponse>
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
                    Code = StatusCodes.Status500InternalServerError,
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