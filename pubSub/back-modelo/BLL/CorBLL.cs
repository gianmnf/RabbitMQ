using System.Collections.Generic;
using back_modelo.BLL.Exceptions;
using back_modelo.DAL.DAO;
using back_modelo.DAL.Models; 
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace back_modelo.BLL
{
    public class CorBLL : ICorBLL
    {
        public string Mensagem = "Mensagem";
        public readonly ICorDAO _corDao;
        public readonly IPessoaDAO _pessoaDao;
        public CorBLL(ICorDAO corDao, IPessoaDAO pessoaDao)
        {
            _corDao = corDao;
            _pessoaDao = pessoaDao;
        }

        public List<Cor> ObterCores()
        {
            var listaCores = _corDao.ObterCores();
            bool isEmpty = !listaCores.Any();

            if(isEmpty)
            {
                this.Mensagem = "Método ObterCores() BLL falhou!";
                return null;
            }
            else
            {
                this.Mensagem = "Método executado corretamente ObterCores() BLL";     
                return listaCores;
            }
        }

        public Cor ObterCorPorId(string idCor)
        {
            var cor = _corDao.ObterCorPorId(idCor);

            if(cor == null)
            {
                this.Mensagem = "Metodo executado incorretamente ObterCorPorId() BLL";
                throw new NotFoundException("Id não encontrado.");
            }

            return cor;
        }

        public Cor ObterCorPorNome(string nomeCor)
        {

            bool validaNomeCor = String.IsNullOrWhiteSpace(nomeCor); 
 
            if(validaNomeCor)
            {
                throw new ArgumentException("Nome não pode ser vazio. ObterCorPorNome() BLL falhou !");
            }

            var cor = _corDao.ObterCorPorNome(nomeCor);

            if(cor == null)
            {        
                this.Mensagem = "Metodo executado incorretamente ObterCorPorNome() BLL"; 
                throw new NotFoundException("Cor não encontrada.");
            }
            
            return cor;
        }

        public void InserirCor(Cor novaCor)
        {
            bool hasAny = (_corDao.ObterCorPorNome(novaCor.NomeCor.ToUpper())) != null;
            bool NomeCor = String.IsNullOrWhiteSpace(novaCor.NomeCor); 
            
            if(NomeCor)
            {
                throw new ArgumentException("Nome não pode ser vazio. InserirCor() BLL falhou !");
            }

            if (!hasAny)
            {
                if(novaCor != null)
                {
                    _corDao.InserirCor(novaCor);
                    this.Mensagem = "Metodo executado corretamente InserirCor() BLL";
                }
            } 
            else 
            {
                this.Mensagem = "Metodo executado incorretamente InserirCor() BLL";
                throw new IntegrityException("Não foi possível efetuar a inserção.");
            }
        }

        public void AtualizarCor(string idCor, Cor novaCor)
        {
            bool hasAny = (_corDao.ObterCorPorNome(novaCor.NomeCor)) != null;

            if (!hasAny)
            {
                try
                {
                    _corDao.AtualizarCor(idCor, novaCor);
                }
                catch (DbUpdateConcurrencyException e)
                {
                    throw new DbConcurrencyException(e.Message);
                }
            } 
            else 
            {
                this.Mensagem = "Metodo executado incorretamente AtualizarCor() BLL";
                throw new IntegrityException("Não foi possível efetuar a atualização.");
            } 
        }

        public void DeletarCor(string idCor)
        {
            var cor = _corDao.ObterCorPorId(idCor);

            bool hasAny = cor != null;
            
            if (!hasAny)
            {
                throw new NotFoundException("Id da cor não encontrado.");
            }

            var listaPessoas = _pessoaDao.ObterPessoasPorCor(cor.NomeCor);
            
            bool hasAnyFuncs = listaPessoas.Any();

            if(!hasAnyFuncs)
            {
                try
                {
                    _corDao.DeletarCor(cor.IdCor);
                }
                catch (DbUpdateException)
                {
                    throw new IntegrityException("Não foi possível efetuar a remoção.");
                }
            } 
            else 
            {
                throw new IntegrityException("Ação não permitida. Existe uma pessoa vinculada a essa cor!");
            }
        }
    }
}
