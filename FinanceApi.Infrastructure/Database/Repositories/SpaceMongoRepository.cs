using FinanceApi.Infrastructure.Abstraction;
using FinanceApi.Infrastructure.Entity.CoreModels;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApi.Infrastructure.Database.Repositories
{
    public class SpaceMongoRepository : ISpaceRepository
    {
        private readonly IMongoCollection<Space> _collection;

        public SpaceMongoRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<Space>("spaces");
        }

        public async Task<Space?> GetByIdAsync(Guid id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<List<Space>> GetAllAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task<List<Space>> GetSpacesAsync(List<Guid> spaceIds) =>
            await _collection.Find(x => spaceIds.Contains(x.Id)).ToListAsync();

        public async Task CreateAsync(Space space) =>
            await _collection.InsertOneAsync(space);

        public async Task UpdateAsync(Guid id, Space space) =>
            await _collection.ReplaceOneAsync(x => x.Id == id, space);

        public async Task DeleteAsync(Guid id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);
    }
}
