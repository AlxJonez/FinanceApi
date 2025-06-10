using FinanceApi.Infrastructure.Entity.CoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApi.Infrastructure.Abstraction
{
    public interface ITransactionRepository
    {
        Task<Transaction?> GetByIdAsync(Guid id);
        Task<List<Transaction>> GetAllAsync();
        Task CreateAsync(Transaction transaction);
        Task UpdateAsync(Guid id, Transaction transaction);
        Task DeleteAsync(Guid id);
        Task<List<Transaction>> SearchByNoteOrLocationAsync(string text);
        Task<List<Transaction>> GetLastTransactions(Guid userId, int offset, int limit);
    }
}
