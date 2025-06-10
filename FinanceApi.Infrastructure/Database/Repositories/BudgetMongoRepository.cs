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
    public class BudgetMongoRepository: IBudgetRepository
    {
        private readonly IMongoCollection<Budget> _collection;

        public BudgetMongoRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<Budget>("budgets");
        }

        public async Task<Budget?> GetByIdAsync(Guid id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<List<Budget>> GetAllAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task CreateAsync(Budget budget) =>
            await _collection.InsertOneAsync(budget);

        public async Task UpdateAsync(Guid id, Budget budget) =>
            await _collection.ReplaceOneAsync(x => x.Id == id, budget);

        public async Task DeleteAsync(Guid id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);

        public async Task<List<Budget>> GetBySpaceIdAsync(Guid spaceId) =>
            await _collection.Find(x => x.SpaceId == spaceId).ToListAsync();
    }
}
