using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApi.Infrastructure.Entity.CoreModels
{
    public class Receipt
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        public string StoreName { get; set; }
        public string Address { get; set; }
        public DateTime Date { get; set; }
        public string ReceiptNumber { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public string PaymentType { get; set; }
        public List<Product> Items { get; set; }
    }
}
