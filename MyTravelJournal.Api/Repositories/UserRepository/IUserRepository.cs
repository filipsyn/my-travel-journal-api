using MyTravelJournal.Api.Models;

namespace MyTravelJournal.Api.Repositories.UserRepository;

public interface IUserRepository
{
   public Task<User?> GetByIdAsync(int userId);

   public Task<User?> GetByUsernameAsync(string username);

   public Task<IEnumerable<User>> GetAllAsync();

   public Task CreateAsync(User user);

   public Task UpdateAsync(User user);

   public Task DeleteAsync(int userId);
}