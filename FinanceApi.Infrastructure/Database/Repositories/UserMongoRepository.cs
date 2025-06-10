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
    public class UserMongoRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _collection;

        public UserMongoRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<User>("users");
        }

        public async Task<User?> GetByIdAsync(Guid id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<List<User>> GetAllAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task CreateAsync(User user) =>
            await _collection.InsertOneAsync(user);

        public async Task UpdateAsync(Guid id, User user) =>
            await _collection.ReplaceOneAsync(x => x.Id == id, user);

        public async Task DeleteAsync(Guid id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);

        public async Task<List<User>> SearchByNameOrEmailAsync(string search) =>
            await _collection.Find(x => x.Name.Contains(search) || x.Email.Contains(search)).ToListAsync();

        public async Task<User?> GetByEmailAsync(string email) =>
            await _collection.Find(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase) == true).FirstOrDefaultAsync();
        public async Task<User?> GetByPhoneAsync(string phone) =>
            await _collection.Find(x => x.Phone == phone).FirstOrDefaultAsync();
    }
}
