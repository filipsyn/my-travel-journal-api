using Microsoft.EntityFrameworkCore;
using MyTravelJournal.Api.Data;
using MyTravelJournal.Api.Models;

namespace MyTravelJournal.Api.Repositories.UserRepository;

public class UserRepository : IUserRepository
{
    private readonly DataContext _db;

    public UserRepository(DataContext db)
    {
        _db = db;
    }

    public async Task<User?> GetByIdAsync(int userId)
    {
        return await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _db.Users.ToListAsync();
    }

    public async Task CreateAsync(User user)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        _db.Entry(user).State = EntityState.Modified;
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int userId)
    {
        var user = await this.GetByIdAsync(userId);
        if (user is null) return;

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
    }
}