using System.Collections.Generic;
using back_modelo.DAL.Models;

namespace back_modelo.DAL.DAO
{
    public interface ICorDAO
    {
        List<Cor> ObterCores();
        Cor ObterCorPorId(string idCor);
        Cor ObterCorPorNome(string nomeCor);
        void InserirCor(Cor novaCor);
        void AtualizarCor(string idCor, Cor novaCor);
        void DeletarCor(string idCor);
    }
}