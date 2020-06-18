using System;
using System.Linq;
using System.Collections.Generic;
using MongoDB.Driver;
using back_modelo.DAL.Models;

namespace back_modelo.DAL.DAO
{
    public class CorDAO : ICorDAO
    {
        private readonly IMongoContext _context;
        public CorDAO(IMongoContext context)
        {
            _context = context;
        }

        public List<Cor> ObterCores()
        {
            var sort = Builders<Cor>.Sort.Ascending(c => c.NomeCor);
            var cores = _context.CollectionCor.Find(c => true).Sort(sort).ToList();

            return cores;
        }

        public Cor ObterCorPorId(string idCor)
        {
            var cor = _context.CollectionCor.Find<Cor>(p => p.IdCor == idCor).FirstOrDefault();

            return cor;
        }

        public Cor ObterCorPorNome(string nomeCor)
        {
            var cor = _context.CollectionCor.Find<Cor>(c => c.NomeCor == nomeCor).FirstOrDefault();

            return cor;
        }

        public void InserirCor(Cor novaCor)
        {
            Cor cor = new Cor{
                NomeCor =  novaCor.NomeCor.TrimStart().TrimEnd().ToUpper()
            };
            
            _context.CollectionCor.InsertOne(cor);
        }

        public void AtualizarCor(string idCor, Cor novaCor)
        {
            var corAtual = _context.CollectionCor.Find(c => c.IdCor == idCor).FirstOrDefault();

            Cor cor = new Cor{
              IdCor = idCor,
              NomeCor =  novaCor.NomeCor.TrimStart().TrimEnd().ToUpper()
            };

            _context.CollectionPessoa.UpdateMany(p =>
                p.Cor == corAtual.NomeCor,
                Builders<Pessoa>.Update.Set(p => p.Cor, novaCor.NomeCor.ToUpper()),
                new UpdateOptions { IsUpsert = false }
            );

            _context.CollectionCor.ReplaceOne(c => c.IdCor == idCor, cor);
        }

        public void DeletarCor(string idCor)
        {
            var cor = _context.CollectionCor.Find(c => c.IdCor == idCor);

            _context.CollectionCor.DeleteOne(p => p.IdCor == idCor);
        }
    }
}