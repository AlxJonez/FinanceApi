using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApi.Infrastructure.Entity.CoreModels
{
    public class Budget
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [BsonElement("spaceId")]
        [BsonRepresentation(BsonType.String)]
        public Guid SpaceId { get; set; }

        /// <summary>
        /// Название бюджета
        /// </summary>
        [BsonElement("name")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// ID родительской папки
        /// </summary>
        [BsonElement("parentId")]
        [BsonRepresentation(BsonType.String)]
        public Guid? ParentId { get; set; }

        /// <summary>
        /// Текущий баланс бюджета
        /// </summary>
        [BsonElement("currentBalance")]
        public decimal CurrentBalance { get; set; }

        /// <summary>
        /// Валюта баланса (может отличаться от валюты пространства)
        /// </summary>
        [BsonElement("currency")]
        [BsonRepresentation(BsonType.String)]
        public Currency Currency { get; set; }

        /// <summary>
        /// включать ли содержимое этого бюджета в общий подсчет баланса
        /// </summary>
        [BsonElement("includeChildren")]
        public bool IncludeChildren { get; set; } = true;

        /// <summary>
        /// Может ли бюджет быть минусовым (например кредитка)
        /// </summary>
        [BsonElement("canBeMinus")]
        public bool CanBeMinus { get; set; }

        [BsonElement("isActive")]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Нужно ли скрывать бюджет от других пользователей внутри спейса
        /// </summary>
        [BsonElement("hideInShared")]
        public bool HideInShared { get; set; } = false;

        /// <summary>
        /// Порядок отображения, для того чтоб двигать в списке
        /// </summary>
        [BsonElement("sortOrder")]
        public int SortOrder { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Является системнім бюджетом или нет
        /// </summary>
        public bool IsSystem { get; set; } = false;
    }
}
