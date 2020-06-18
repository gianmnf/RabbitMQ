using System;

namespace back_modelo.BLL.Exceptions
{
    public class  DbConcurrencyException: ApplicationException
    {
        public DbConcurrencyException(string message) : base(message)
        {
        }
    }

}