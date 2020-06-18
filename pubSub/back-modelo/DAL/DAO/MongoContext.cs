using MongoDB.Driver;
using Microsoft.Extensions.Options;
using back_modelo.DAL.Models;

namespace back_modelo.DAL.DAO
{
    public class MongoContext:IMongoContext
    {
        private readonly IMongoDatabase _db;

        public MongoContext(IOptions<Configuracoes> options, IMongoClient client)
        {
            _db = client.GetDatabase(options.Value.Database);
        }
        public IMongoCollection<Pessoa> CollectionPessoa => _db.GetCollection<Pessoa>("Pessoa");
        public IMongoCollection<Cor> CollectionCor => _db.GetCollection<Cor>("Cor");
    }
}