using FinanceApi.Infrastructure.Entity.CoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApi.Infrastructure.Abstraction
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<List<User>> GetAllAsync();
        Task CreateAsync(User user);
        Task UpdateAsync(Guid id, User user);
        Task DeleteAsync(Guid id);
        Task<List<User>> SearchByNameOrEmailAsync(string search);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByPhoneAsync(string phone);
    }
}
