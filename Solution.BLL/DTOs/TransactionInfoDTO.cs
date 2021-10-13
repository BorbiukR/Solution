using Solution.DAL.Enum;

namespace Solution.BLL.DTO
{
    public class TransactionInfoDTO
    {
        public int TransactionId { get; set; }

        public Status Status { get; set; }

        public TransactionType Type { get; set; }

        public string ClientName { get; set; }

        public string Amount { get; set; }
    }
}