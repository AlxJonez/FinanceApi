using FinanceApi.Infrastructure.Entity.CoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApi.Infrastructure.Abstraction
{
    public interface ISpaceUserRepository
    {
        Task<SpaceUser?> GetByIdAsync(Guid id);
        Task<List<SpaceUser>> GetAllAsync();
        Task CreateAsync(SpaceUser entity);
        Task UpdateAsync(Guid id, SpaceUser entity);
        Task DeleteAsync(Guid id);
        Task<List<SpaceUser>> GetByUserIdAsync(Guid userId);
        Task<List<SpaceUser>> GetBySpaceIdAsync(Guid spaceId);
        Task<SpaceUser?> GetSpaceUserAsync(Guid userId, Guid spaceId);
        Task DeleteAllSpaceAsync(Guid spaceId);
    }
}
