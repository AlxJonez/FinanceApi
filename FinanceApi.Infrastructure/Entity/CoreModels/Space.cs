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
    /// Пространство пользователя
    /// </summary>
    public class Space
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("ownerId")]
        [BsonRepresentation(BsonType.String)]
        public Guid OwnerId { get; set; }

        /// <summary>
        /// Нужно сделать справочник валют
        /// </summary>
        [BsonElement("currency")]
        [BsonRepresentation(BsonType.String)]
        public Currency Currency { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("isArchived")]
        public bool IsArchived { get; set; } = false;

        [BsonElement("archivedAt")]
        public DateTime? ArchivedAt { get; set; } = null;
    }
}
