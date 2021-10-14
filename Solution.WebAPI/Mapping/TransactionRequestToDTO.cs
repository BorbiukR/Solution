using Solution.BLL.DTO;
using Solution.Contracts.Requests.Queries;

namespace Solution.WebAPI.Mapping
{
    public static class TransactionRequestToDTO
    {
        public static GetAllByFilterDTO DomainToRequestFilter(this GetAllDataQuery transaction)
        {
            if (transaction != null)
            {
                return new GetAllByFilterDTO
                {
                    Status = transaction.Status,
                    TransactionType = transaction.TransactionType
                };
            }

            return null;
        }
    }
}