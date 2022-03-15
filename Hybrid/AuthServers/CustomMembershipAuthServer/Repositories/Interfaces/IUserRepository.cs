using CustomMembershipAuthServer.Models;

namespace CustomMembershipAuthServer.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> ValidateAsync(string email, string password);
        Task<AppUser?> GetByIdAsync(int id);
        Task<AppUser?> GetByEmailAsync(string email);
    }
}