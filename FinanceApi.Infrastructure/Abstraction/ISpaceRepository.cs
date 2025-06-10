using FinanceApi.Infrastructure.Entity.CoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApi.Infrastructure.Abstraction
{
    public interface ISpaceRepository
    {
        Task<Space?> GetByIdAsync(Guid id);
        Task<List<Space>> GetAllAsync();
        Task CreateAsync(Space space);
        Task UpdateAsync(Guid id, Space space);
        Task DeleteAsync(Guid id);
        Task<List<Space>> GetSpacesAsync(List<Guid> spaceIds);
    }
}
