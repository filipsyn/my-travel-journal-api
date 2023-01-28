using AutoMapper;
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
            Data = _mapper.Map<UserDetailsResponse>(user)
        };
    }

    public async Task<ServiceResponse<IEnumerable<UserDetailsResponse>>> GetAllAsync()
    {
        var users = await _db.Users.ToListAsync();
        return new ServiceResponse<IEnumerable<UserDetailsResponse>>
        {
            Success = true,
            Data = _mapper.Map<IEnumerable<UserDetailsResponse>>(users)
        };
    }


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
                Data = null,
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
            Data = null,
            Success = true,
            Details = new StatusDetails
            {
                Code = StatusCodes.Status204NoContent
            }
        };
    }

    /*
    public Task<ServiceResponse<UserDetailsResponse>> UpdateAsync(int id)
    {
    }

    public Task<ServiceResponse<UserDetailsResponse>> DeleteByIdAsync(int id)
    {
    }
    */
}