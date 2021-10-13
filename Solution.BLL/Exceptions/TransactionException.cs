using System;

namespace Solution.BLL.Exception
{
    public class TransactionException : SystemException
    {
        public TransactionException() { }

        public TransactionException(string message) : base(message) { }

        public TransactionException(string message, RankException innerException) : base(message, innerException) { }
    }
}