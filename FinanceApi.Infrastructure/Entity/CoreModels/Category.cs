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
    /// Категория транзакций, как для поступлений так и для расходов
    /// </summary>
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        /// <summary>
        /// Если системная, то владелец - null
        /// </summary>
        [BsonElement("ownerUserId")]
        [BsonRepresentation(BsonType.String)]
        public Guid? OwnerUserId { get; set; }

        [BsonElement("type")]
        public string Type { get; set; } = "Expense";

        /// <summary>
        /// Название которое будет использоваться по умолчанию
        /// </summary>
        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("parentId")]
        [BsonRepresentation(BsonType.String)]
        public Guid? ParentId { get; set; }

        /// <summary>
        /// Дефолтная папка
        /// </summary>
        [BsonElement("isSystem")]
        public bool IsSystem { get; set; } = false;

        /// <summary>
        /// Настройки категории по умолчанию
        /// </summary>
        [BsonElement("defaultSetting")]
        public CategorySetting? DefaultSetting { get; set; } = null;

        /// <summary>
        /// Список названия для разных языков
        /// </summary>
        public Dictionary<string, string> NamesLang { get; set; } = new();
    }
}
