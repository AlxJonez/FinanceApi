using FinanceApi.Infrastructure.Entity.CoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApi.Infrastructure.Abstraction
{
   public interface ICategoryRepository
   {
        Task<Category?> GetByIdAsync(Guid id);
        Task<List<Category>> GetAllAsync();
        Task CreateAsync(Category category);
        Task UpdateAsync(Guid id, Category category);
        Task DeleteAsync(Guid id);
        Task<List<Category>> SearchByNameAsync(string name);
        Task<List<Category>> GetAllForUserAsync(Guid? userId);
    }
}
