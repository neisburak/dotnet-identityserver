using CustomMembershipAuthServer.Data;
using CustomMembershipAuthServer.Models;
using CustomMembershipAuthServer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CustomMembershipAuthServer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AppUser?> GetByEmailAsync(string email) => await _context.Users.FirstOrDefaultAsync(f => f.Email == email);

        public async Task<AppUser?> GetByIdAsync(int id) => await _context.Users.FirstOrDefaultAsync(f => f.Id == id);

        public async Task<bool> ValidateAsync(string email, string password) => await _context.Users.AnyAsync(f => f.Email == email && f.Password == password);
    }
}