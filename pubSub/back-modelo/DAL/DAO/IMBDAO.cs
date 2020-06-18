using System.Collections.Generic;
using back_modelo.DAL.Models;

namespace back_modelo.DAL.DAO
{
    public interface IMBDAO
    {
        void EnviarConsumer(string tipo, string info);
    }
}