using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace back_modelo.DAL.Models
{
    public class Cor
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdCor { get; set; }

        [BsonElement("NomeCor")]
        public string NomeCor { get; set; }
    }
}