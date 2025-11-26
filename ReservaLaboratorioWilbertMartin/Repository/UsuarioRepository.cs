using ReservaLaboratorioWilbertMartin.Data;
using ReservaLaboratorioWilbertMartin.Models;

namespace ReservaLaboratorioWilbertMartin.Repository
{
    public class UsuarioRepository(AppDbContext context) : IUsuarioRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<User> AddAsync(User user)
        {
            _context.users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.users.FindAsync(email);
        }

        public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return await _context.users.FindAsync(refreshToken);
        }

        public async Task<User?> GetUserByUserName(string userName)
        {
            return await _context.users.FindAsync(userName);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public bool ValidatePassWord(User user, string passWord)
        {
            return user.Password == passWord;
        }
    }
}
