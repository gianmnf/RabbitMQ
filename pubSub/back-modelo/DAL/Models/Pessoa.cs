using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace back_modelo.DAL.Models
{
    public class Pessoa
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdPessoa { get; set; }

        [BsonElement("Nome")]
        public string Nome { get; set; }

        [BsonElement("Idade")]
        public int Idade { get; set; }

        [BsonElement("CPF")]
        public string CPF { get; set; }

        [BsonElement("RG")]
        public string RG { get; set; }

        [BsonElement("Data_Nasc")]
        public string Data_Nasc { get; set; }

        [BsonElement("Signo")]
        public string Signo { get; set; }

        [BsonElement("Mae")]
        public string Mae { get; set; }
        
        [BsonElement("Pai")]
        public string Pai { get; set; }

        [BsonElement("Email")]
        public string Email { get; set; }

        [BsonElement("Senha")]
        public string Senha { get; set; }

        [BsonElement("CEP")]
        public string CEP { get; set; }

        [BsonElement("Endereco")]
        public string Endereco { get; set; }

        [BsonElement("Numero")]
        public int Numero { get; set; }

        [BsonElement("Bairro")]
        public string Bairro { get; set; }

        [BsonElement("Cidade")]
        public string Cidade { get; set; }

        [BsonElement("Estado")]
        public string Estado { get; set; }

        [BsonElement("Telefone_Fixo")]
        public string Telefone_Fixo { get; set; }

        [BsonElement("Celular")]
        public string Celular { get; set; }

        [BsonElement("Altura")]
        public string Altura { get; set; }

        [BsonElement("Peso")]
        public double Peso { get; set; }

        [BsonElement("Tipo_Sanguineo")]
        public string Tipo_Sanguineo { get; set; }

        [BsonElement("Cor")]
        public string Cor { get; set; }
    }
}