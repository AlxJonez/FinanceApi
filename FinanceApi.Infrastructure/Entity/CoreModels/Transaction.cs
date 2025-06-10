using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApi.Infrastructure.Entity.CoreModels
{
    /// <summary>
    /// Транзакция средств. Поступление или расход
    /// </summary>
    public class Transaction
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [BsonElement("userId")]
        [BsonRepresentation(BsonType.String)]
        public Guid UserId { get; set; }

        [BsonElement("spaceId")]
        [BsonRepresentation(BsonType.String)]
        public Guid SpaceId { get; set; }

        [BsonElement("budgetId")]
        [BsonRepresentation(BsonType.String)]
        public Guid BudgetId { get; set; }

        [BsonElement("categoryId")]
        [BsonRepresentation(BsonType.String)]
        public Guid CategoryId { get; set; }

        [BsonElement("amount")]
        public decimal Amount { get; set; }

        [BsonRepresentation(BsonType.String)]
        [BsonElement("currency")]
        public Currency Currency { get; set; }

        /// <summary>
        /// Тип: приход \ расход. Expense / Income
        /// </summary>
        [BsonElement("type")]
        public TransactionType Type { get; set; }

        /// <summary>
        /// Пользовательский комментарий
        /// </summary>
        [BsonElement("note")]
        public string? Note { get; set; }

        /// <summary>
        /// Координаты
        /// </summary>
        [BsonElement("locationLat")]
        public double? LocationLat { get; set; }

        [BsonElement("locationLon")]
        public double? LocationLon { get; set; }

        [BsonElement("locationName")]
        public string? LocationName { get; set; }

        /// <summary>
        /// Время совершения транзакции. По умолчанию делать текущее, но с возможностью задать дату
        /// </summary>
        [BsonElement("date")]
        public DateTime Date { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Когда была добавлена запись
        /// </summary>
        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Разовый платеж или повторяющийся
        /// </summary>
        [BsonElement("isRecurring")]
        public bool IsRecurring { get; set; } = false;
    }
}
