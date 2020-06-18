using System.Collections.Generic;
using System.Linq;
using back_modelo.DAL.Models;
using MongoDB.Driver;

namespace back_modelo.DAL.DAO
{
    public class SeedingService
    {
        private readonly IMongoContext _context;

        public SeedingService(IMongoContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            /* if (_context.CollectionPessoa.Find(unid => true).ToList().Count != 0)
            {
                return; // DB has been seeded
            }

            Pessoa root = new Pessoa();
            root.Nome = "Teste Pessoa";
            root.Idade = 20;
            root.CPF = "111.222.333-44";
            root.RG = "MG-11.222.333";
            root.Data_Nasc = "01/01/2000";
            root.Signo = "Teste Signo";
            root.Mae = "Teste Mãe";
            root.Pai = "Teste Pai";
            root.Email = "email@teste.com";
            root.Senha = "@123456";
            root.CEP = "38700-000";
            root.Endereco = "Teste Endereço";
            root.Numero = 100;
            root.Bairro = "Teste Bairro";
            root.Cidade = "Teste Cidade";
            root.Estado = "Teste Estado";
            root.Telefone_Fixo = "(00)0000-0000";
            root.Celular = "(00)00000-0000";
            root.Altura = "1,80";
            root.Peso = 70.5;
            root.Tipo_Sanguineo = "Teste Tipo Sanguineo";
            root.Cor =  "Teste Cor";

            List<Pessoa> pessoas = new List<Pessoa>();
            pessoas.Add(root);

            _context.CollectionPessoa.InsertMany(pessoas); */
        }
    }
}
