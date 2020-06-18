using System;

namespace back_modelo.BLL.Exceptions
{
    public class  IntegrityException : ApplicationException
    {
        public IntegrityException(string message) : base(message)
        {
        }
    }

}