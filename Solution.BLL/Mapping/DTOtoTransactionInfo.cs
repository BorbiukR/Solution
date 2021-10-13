using Solution.BLL.DTO;
using Solution.DAL.Entities;

namespace Solution.BLL.Mapping
{
    public static class DTOtoTransactionInfo
    {
        public static TransactionInfo ToDomain(this TransactionInfoDTO transaction)
        {
            if (transaction != null)
            {
                return new TransactionInfo
                {
                    Id = transaction.TransactionId,
                    Status = transaction.Status,
                    Type = transaction.Type,
                    ClientName = transaction.ClientName,
                    Amount = transaction.Amount
                };
            }

            return null;
        }
    }
}