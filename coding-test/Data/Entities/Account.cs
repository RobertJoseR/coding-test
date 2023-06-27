using System.ComponentModel.DataAnnotations;

namespace coding_test.Data.Entities
{
    public class Account
    {

        [Key]
        public int AccountNumber { get; set; }

        public string AccountType { get; set; }

        public int ClientNumber { get; set; }
        public Customer Customer { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}
