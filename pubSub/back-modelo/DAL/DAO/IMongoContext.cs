using back_modelo.DAL.Models;
using MongoDB.Driver;

namespace back_modelo.DAL.DAO
{
    public interface IMongoContext
    {
        IMongoCollection<Pessoa> CollectionPessoa { get; }
        IMongoCollection<Cor> CollectionCor { get; }
    }
}