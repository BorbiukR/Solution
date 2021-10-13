using Solution.BLL.DTO;
using Solution.DAL.Entities;

namespace Solution.BLL.Mapping
{
    public static class TransactionInfoToDTO
    {
        public static TransactionInfoDTO ToDto(this TransactionInfo transaction)
        {
            if (transaction != null)
            {
                return new TransactionInfoDTO
                {
                    TransactionId = transaction.Id,
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