using Solution.DAL.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solution.DAL.Entities
{
    public class TransactionInfo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int Id { get; set; }

        public Status Status { get; set; }

        public TransactionType Type { get; set; }

        public string ClientName { get; set; }

        public string Amount { get; set; }
    }
}