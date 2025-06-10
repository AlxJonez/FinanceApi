using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApi.Infrastructure.Entity.CoreModels
{
    public class CategorySetting
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [BsonElement("categoryId")]
        [BsonRepresentation(BsonType.String)]
        public Guid CategoryId { get; set; }

        [BsonElement("ownerUserId")]
        [BsonRepresentation(BsonType.String)]
        public Guid OwnerUserId { get; set; }

        /// <summary>
        /// Лимит на расходы для этой категории в месяц\неделю
        /// </summary>
        [BsonElement("limitPerPeriod")]
        public decimal? LimitPerPeriod { get; set; }

        /// <summary>
        /// Период для которого установлен лимит
        /// </summary>
        [BsonElement("limitPeriod")]
        public string? LimitPeriod { get; set; }

        /// <summary>
        /// Цель снизить расход для категории на ХХ% в месяц
        /// </summary>
        [BsonElement("reduceGoal")]
        public decimal? ReduceGoal { get; set; }

        /// <summary>
        /// Порядок для сортировки
        /// </summary>
        [BsonElement("sortOrder")]
        public int SortOrder { get; set; }
    }
}
