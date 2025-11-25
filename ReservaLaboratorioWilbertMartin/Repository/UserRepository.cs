using Microsoft.EntityFrameworkCore;
using ReservaLaboratorioWilbertMartin.Data;
using ReservaLaboratorioWilbertMartin.Models;

namespace ReservaLaboratorioWilbertMartin.Repository
{
    public class UserRepository(AppDbContext context) : IUserRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<User> AddAsync(User user)
        {
            var entry = await _context.users.AddAsync(user);
            return entry.Entity;
            /*    _context.users.Add(user); await _context.SaveChangesAsync();   return user;*/
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == email);
        }


        public async Task<User?> GetUserByUserName(string userName)
        {
            return await _context.users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public bool ValidatePassWord(User user, string passWord)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(passWord, user.Password);

        }


        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }



        public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
        {

            return await _context.users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }
    }
}
