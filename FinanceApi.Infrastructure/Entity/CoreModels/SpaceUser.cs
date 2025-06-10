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
    /// Связка пользователей с пространством
    /// </summary>
    public class SpaceUser
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        [BsonElement("userId")]
        [BsonRepresentation(BsonType.String)]
        public Guid UserId { get; set; }

        [BsonElement("spaceId")]
        [BsonRepresentation(BsonType.String)]
        public Guid SpaceId { get; set; }

        /// <summary>
        /// Роль пользователя внутри пространства: участие, редактирование и т.д.
        /// owner, editor, viewer
        /// </summary>
        [BsonElement("role")]
        public string Role { get; set; } = "owner";

        /// <summary>
        /// Состояние пользователя для пространства. Подтвердил приглашение или нет, для создавшего - Accepted
        /// </summary>
        [BsonElement("inviteStatus")]
        public string InviteStatus { get; set; } = "Pending";
    }
}
