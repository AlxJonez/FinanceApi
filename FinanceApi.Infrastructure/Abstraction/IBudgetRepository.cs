using FinanceApi.Infrastructure.Entity.CoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApi.Infrastructure.Abstraction
{
    public interface IBudgetRepository
    {
        Task<Budget?> GetByIdAsync(Guid id);
        Task<List<Budget>> GetAllAsync();
        Task CreateAsync(Budget budget);
        Task UpdateAsync(Guid id, Budget budget);
        Task DeleteAsync(Guid id);
        Task<List<Budget>> GetBySpaceIdAsync(Guid spaceId);
    }
}