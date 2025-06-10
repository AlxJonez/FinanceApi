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
    public class TransactionMongoRepository : ITransactionRepository
    {
        private readonly IMongoCollection<Transaction> _collection;

        public TransactionMongoRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<Transaction>("transactions");
        }

        public async Task<Transaction?> GetByIdAsync(Guid id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<List<Transaction>> GetAllAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task CreateAsync(Transaction transaction) =>
            await _collection.InsertOneAsync(transaction);

        public async Task UpdateAsync(Guid id, Transaction transaction) =>
            await _collection.ReplaceOneAsync(x => x.Id == id, transaction);

        public async Task DeleteAsync(Guid id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);

        public async Task<List<Transaction>> SearchByNoteOrLocationAsync(string text) =>
            await _collection.Find(x => x.Note.Contains(text) || x.LocationName.Contains(text)).ToListAsync();

        public async Task<List<Transaction>> GetLastTransactions(Guid userId, int offset, int limit)
        {
            var builder = Builders<Transaction>.Filter;
            var filter = builder.Empty;

            filter = builder.Eq(x => x.UserId, userId);


            var transactions = await _collection.Find(filter)
                .SortByDescending(x => x.Date)
                .Skip(offset)
                .Limit(limit)
                .ToListAsync();

            return transactions;
        }
    }
}
