using Solution.DAL.Enum;

namespace Solution.Contracts.Requests.Queries
{
    public class GetAllDataQuery
    {
        public Status? Status { get; set; }

        public TransactionType? TransactionType { get; set; }
    }
}