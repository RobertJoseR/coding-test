
using System.ComponentModel.DataAnnotations;

namespace coding_test.Data.Entities
{
    public class Transaction : IAuditCreate
    {
        [Key]
        public int TransactionId { get; set; }

        public int AccountNumber{ get; set; }
        public decimal Amount { get; set; }
        public string CreatedBy { get; set; }
        public DateTime AddedOn { get; set; }

        public Account Account { get; set; }
    }
}
