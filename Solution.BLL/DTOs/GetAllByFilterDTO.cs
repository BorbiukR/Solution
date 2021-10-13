using Solution.DAL.Enum;

namespace Solution.BLL.DTO
{
    public class GetAllByFilterDTO
    {
        public Status? Status { get; set; }

        public TransactionType? TransactionType { get; set; }
    }
}