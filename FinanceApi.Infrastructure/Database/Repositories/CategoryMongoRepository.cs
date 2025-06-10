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
    public class CategoryMongoRepository : ICategoryRepository
    {
        private readonly IMongoCollection<Category> _collection;

        public CategoryMongoRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<Category>("categories");
        }

        public async Task<Category?> GetByIdAsync(Guid id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<List<Category>> GetAllAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task<List<Category>> GetAllForUserAsync(Guid? userId)
        {
            var builder = Builders<Category>.Filter;
            var filter = builder.Empty;

            if (userId.HasValue)
                filter = builder.Or(
                    builder.Eq(x => x.IsSystem, true),
                    builder.Eq(x => x.OwnerUserId, userId.Value)
                );
            else
                filter = builder.Eq(x => x.IsSystem, true);

            var categories = await _collection.Find(filter).ToListAsync();
            return categories;
        }

        public async Task CreateAsync(Category category) =>
            await _collection.InsertOneAsync(category);

        public async Task UpdateAsync(Guid id, Category category) =>
            await _collection.ReplaceOneAsync(x => x.Id == id, category);

        public async Task DeleteAsync(Guid id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);

        public async Task<List<Category>> SearchByNameAsync(string name) =>
            await _collection.Find(x => x.Name.ToLower().Contains(name.ToLower())).ToListAsync();
    }
}
