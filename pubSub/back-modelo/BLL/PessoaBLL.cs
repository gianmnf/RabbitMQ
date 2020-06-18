using System.Collections.Generic;
using back_modelo.BLL.Exceptions;
using back_modelo.DAL.DAO;
using back_modelo.DAL.Models; 
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace back_modelo.BLL
{
    public class PessoaBLL : IPessoaBLL
    {
        public string Mensagem = "Mensagem";
        public readonly IPessoaDAO _pessoaDao;

        public readonly IMBDAO _mbDao;

        public PessoaBLL(IPessoaDAO pessoaDao, IMBDAO mbDao)
        {
            _pessoaDao = pessoaDao;
            _mbDao = mbDao;
        }

        public List<Pessoa> ObterPessoas()
        {
            var listaPessoas = _pessoaDao.ObterPessoas();
            bool isEmpty = !listaPessoas.Any();

            if(isEmpty)
            {
                this.Mensagem = "Método ObterPessoas() BLL falhou!";
                return null;
            }
            else
            {
                this.Mensagem = "Método executado corretamente ObterPessoas() BLL";     
                return listaPessoas;
            }
        }

        public Pessoa ObterPessoaPorId(string idPessoa)
        {
            var pessoa = _pessoaDao.ObterPessoaPorId(idPessoa);

            if(pessoa == null)
            {
                this.Mensagem = "Metodo executado incorretamente ObterPessoaPorId() BLL";
                throw new NotFoundException("Id não encontrado.");
            }
            
            return pessoa;
        }

        public Pessoa ObterPessoaPorCPF(string cpf)
        {
            var pessoa = _pessoaDao.ObterPessoaPorCPF(cpf);

            if(pessoa == null)
            {
                this.Mensagem = "Metodo executado incorretamente ObterPessoaPorCPF() BLL";
                throw new NotFoundException("CPF não encontrado.");
            }
            
            return pessoa;
        }

        public List<Pessoa> ObterPessoasPorCor(string cor)
        {
            var listaPessoas = _pessoaDao.ObterPessoasPorCor(cor);
            bool isEmpty = !listaPessoas.Any();

            if(isEmpty)
            {
                this.Mensagem = "Método ObterPessoasPorCor() BLL falhou!";
                return null;
            }
            else
            {
                this.Mensagem = "Método executado corretamente ObterPessoasPorCor() BLL";     
                return listaPessoas;
            }
        }

        public int ObterTotalPessoas()
        {
            var verifica = _pessoaDao.ObterTotalPessoas();
            
            if (verifica == 0)
            {
                return 0;
            }
            
            this.Mensagem = "Método executado corretamente ObterTotalPessoas() BLL";
            
            return verifica;
        }


        public void InserirPessoa(Pessoa novaPessoa)
        {
            bool hasAny = (_pessoaDao.ObterPessoaPorCPF(novaPessoa.CPF)) != null;
            
            if (!hasAny)
            {
                if(novaPessoa != null)
                {
                    _pessoaDao.InserirPessoa(novaPessoa);
                    _mbDao.EnviarConsumer("I", novaPessoa.Nome);
                }
            } 
            else 
            {
                this.Mensagem = "Metodo executado incorretamente InserirPessoa() BLL";
                throw new IntegrityException("Não foi possível efetuar a inserção.");
            } 
        }

        public void AtualizarPessoa(string idPessoa, Pessoa novaPessoa)
        {
            var procurado = _pessoaDao.ObterPessoaPorCPF(novaPessoa.CPF);

            if(procurado != null){

                if(procurado.IdPessoa == idPessoa)
                    {
                        try
                        {
                            _pessoaDao.AtualizarPessoa(idPessoa, novaPessoa);
                            _mbDao.EnviarConsumer("A", idPessoa);
                        }
                        catch (DbUpdateConcurrencyException e)
                        {
                            throw new DbConcurrencyException(e.Message);
                        }
                    } 
                    else 
                    {
                        throw new IntegrityException("Não foi possível efetuar a atualização.");
                    }
                } 
            else 
            {
                try
                {
                    _pessoaDao.AtualizarPessoa(idPessoa, novaPessoa);
                    _mbDao.EnviarConsumer("A", idPessoa);
                }
                catch (DbUpdateConcurrencyException e)
                {
                    throw new DbConcurrencyException(e.Message);
                }
            }
        }

        public void DeletarPessoa(string idPessoa)
        {
            var pessoa = _pessoaDao.ObterPessoaPorId(idPessoa);

            bool hasAny = pessoa != null;
            if (!hasAny)
            {
                throw new NotFoundException("Id não foi encontrado.");
            }
            try 
            {
                _pessoaDao.DeletarPessoa(pessoa.IdPessoa);
                _mbDao.EnviarConsumer("D", idPessoa);
            }
            catch (DbUpdateException)
            {
                throw new IntegrityException("Não foi possível efetuar a remoção.");
            }
        }
    }
}
