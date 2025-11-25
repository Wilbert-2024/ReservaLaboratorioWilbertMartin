using ReservaLaboratorioWilbertMartin.Models;

namespace ReservaLaboratorioWilbertMartin.Repository
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User user);
        Task<User?> GetUserByEmail(string email);
        Task<User?> GetUserByUserName(string userName);
        bool ValidatePassWord(User user, string passWord);
        Task SaveAsync();
        Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
    }
}
