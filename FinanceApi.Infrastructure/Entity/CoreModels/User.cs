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
    /// Пользователь программы
    /// </summary>
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [BsonElement("email")]
        public string? Email { get; set; }

        [BsonElement("phone")]
        public string? Phone { get; set; }

        [BsonElement("name")]
        public string Name { get; set; } = null!;

        [BsonElement("passwordHash")]
        public string? PasswordHash { get; set; }

        /// <summary>
        /// почта, телефон, эплид, гуглид
        /// </summary>
        [BsonElement("authProvider")]
        public string AuthProvider { get; set; }

        [BsonElement("avatarUrl")]
        public string? AvatarUrl { get; set; }

        /// <summary>
        /// Бесплатный, платный, забокированный, приостановленый
        /// </summary>
        [BsonElement("status")]
        public string Status { get; set; } = "Free";

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("lastLoginAt")]
        public DateTime? LastLoginAt { get; set; }

        /// <summary>
        /// Премиум проплачен до
        /// </summary>
        [BsonElement("paidTo")]
        public DateTime? PaidTo { get; set; }
    }
}
