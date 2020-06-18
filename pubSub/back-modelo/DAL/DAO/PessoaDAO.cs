using System;
using System.Linq;
using System.Collections.Generic;
using MongoDB.Driver;
using back_modelo.DAL.Models;

namespace back_modelo.DAL.DAO
{
    public class PessoaDAO : IPessoaDAO
    {
        private readonly IMongoContext _context;
        public PessoaDAO(IMongoContext context)
        {
            _context = context;
        }

        public List<Pessoa> ObterPessoas()
        {
            var sort = Builders<Pessoa>.Sort.Ascending(p => p.Nome);
            var pessoas = _context.CollectionPessoa.Find(p => true).Sort(sort).ToList();

            return pessoas;
        }

        public Pessoa ObterPessoaPorId(string idPessoa)
        {
            var pessoa = _context.CollectionPessoa.Find<Pessoa>(p => p.IdPessoa == idPessoa).FirstOrDefault();

            return pessoa;
        }

        public Pessoa ObterPessoaPorCPF(string cpf)
        {
            var pessoa = _context.CollectionPessoa.Find<Pessoa>(p => p.CPF == cpf).FirstOrDefault();

            return pessoa;
        }

         public List<Pessoa> ObterPessoasPorCor(string cor)
        {
            var sort = Builders<Pessoa>.Sort.Ascending(p => p.Nome);
            var pessoas = _context.CollectionPessoa.Find<Pessoa>(p => p.Cor == cor).Sort(sort).ToList();

            return pessoas;
        }

        public int ObterTotalPessoas()
        {
            var quantidade = _context.CollectionPessoa.Find(pessoa => true).CountDocuments();
            var resultado = Convert.ToInt32(quantidade);
            
            return resultado;
        }

        public void InserirPessoa(Pessoa novaPessoa)
        {
            Pessoa pessoa = new Pessoa{
                Nome = novaPessoa.Nome.TrimStart().TrimEnd().ToUpper(),
                Idade = novaPessoa.Idade,
                CPF = novaPessoa.CPF,
                RG = novaPessoa.RG,
                Data_Nasc = novaPessoa.Data_Nasc,
                Signo = novaPessoa.Signo,
                Mae = novaPessoa.Mae.TrimStart().TrimEnd(),
                Pai = novaPessoa.Pai.TrimStart().TrimEnd(),
                Email = novaPessoa.Email,
                Senha = novaPessoa.Senha,
                CEP = novaPessoa.CEP,
                Endereco = novaPessoa.Endereco,
                Numero = novaPessoa.Numero,
                Bairro = novaPessoa.Bairro,
                Cidade = novaPessoa.Cidade,
                Estado = novaPessoa.Estado,
                Telefone_Fixo = novaPessoa.Telefone_Fixo,
                Celular = novaPessoa.Celular,
                Altura = novaPessoa.Altura,
                Peso = novaPessoa.Peso,
                Tipo_Sanguineo = novaPessoa.Tipo_Sanguineo,
                Cor =  novaPessoa.Cor.TrimStart().TrimEnd().ToUpper()
            };

            var buscaCor = _context.CollectionCor.Find<Cor>(c => c.NomeCor == novaPessoa.Cor.ToUpper()).CountDocuments();
            var resultado = Convert.ToInt32(buscaCor);

            if (resultado == 0){
                Cor cor = new Cor{
                    NomeCor =  novaPessoa.Cor.TrimStart().TrimEnd().ToUpper()
                };
            
                _context.CollectionCor.InsertOne(cor);
            }

            _context.CollectionPessoa.InsertOne(pessoa);
        }

        public void AtualizarPessoa(string idPessoa, Pessoa novaPessoa)
        {
            Pessoa pessoa = new Pessoa{
                IdPessoa = idPessoa,
                Nome = novaPessoa.Nome.TrimStart().TrimEnd().ToUpper(),
                Idade = novaPessoa.Idade,
                CPF = novaPessoa.CPF,
                RG = novaPessoa.RG,
                Data_Nasc = novaPessoa.Data_Nasc,
                Signo = novaPessoa.Signo,
                Mae = novaPessoa.Mae.TrimStart().TrimEnd(),
                Pai = novaPessoa.Pai.TrimStart().TrimEnd(),
                Email = novaPessoa.Email,
                Senha = novaPessoa.Senha,
                CEP = novaPessoa.CEP,
                Endereco = novaPessoa.Endereco,
                Numero = novaPessoa.Numero,
                Bairro = novaPessoa.Bairro,
                Cidade = novaPessoa.Cidade,
                Estado = novaPessoa.Estado,
                Telefone_Fixo = novaPessoa.Telefone_Fixo,
                Celular = novaPessoa.Celular,
                Altura = novaPessoa.Altura,
                Peso = novaPessoa.Peso,
                Tipo_Sanguineo = novaPessoa.Tipo_Sanguineo,
                Cor =  novaPessoa.Cor
            };

            _context.CollectionPessoa.ReplaceOne(p => p.IdPessoa == idPessoa, pessoa);
        }

        public void DeletarPessoa(string idPessoa)
        {
            var pessoa = _context.CollectionPessoa.Find(p => p.IdPessoa == idPessoa);

            _context.CollectionPessoa.DeleteOne(p => p.IdPessoa == idPessoa);
        }
    }
}