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
    public class SpaceUserMongoRepository : ISpaceUserRepository
    {
        private readonly IMongoCollection<SpaceUser> _collection;

        public SpaceUserMongoRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<SpaceUser>("space_users");
        }

        public async Task<SpaceUser?> GetByIdAsync(Guid id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<SpaceUser?> GetSpaceUserAsync(Guid userId, Guid spaceId)
        {
            return await _collection.Find(x => x.UserId == userId && x.SpaceId == spaceId).FirstOrDefaultAsync();
        }

        public async Task<List<SpaceUser>> GetAllAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task CreateAsync(SpaceUser entity) =>
            await _collection.InsertOneAsync(entity);

        public async Task UpdateAsync(Guid id, SpaceUser entity) =>
            await _collection.ReplaceOneAsync(x => x.Id == id, entity);

        public async Task DeleteAsync(Guid id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);

        public async Task DeleteAllSpaceAsync(Guid spaceId) =>
            await _collection.DeleteManyAsync(x => x.SpaceId == spaceId);


        public async Task<List<SpaceUser>> GetByUserIdAsync(Guid userId) =>
            await _collection.Find(x => x.UserId == userId).ToListAsync();

        public async Task<List<SpaceUser>> GetBySpaceIdAsync(Guid spaceId) =>
            await _collection.Find(x => x.SpaceId == spaceId).ToListAsync();
    }
}
