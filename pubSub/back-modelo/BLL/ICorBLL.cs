using System.Collections.Generic;
using back_modelo.DAL.Models;

namespace back_modelo.BLL
{
    public interface ICorBLL
    {
        List<Cor> ObterCores();
        Cor ObterCorPorId(string idCor);
        Cor ObterCorPorNome(string nomeCor);
        void InserirCor(Cor novaCor);
        void AtualizarCor(string idCor, Cor novaCor);
        void DeletarCor(string idCor);
    }
}