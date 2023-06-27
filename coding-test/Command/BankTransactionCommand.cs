using System.ComponentModel.DataAnnotations;

namespace coding_test.Command
{
    public class BankTransactionCommand
    {
        [Required]
        public int AccountNumber { get; set; }
        
        [Required]
        public decimal Amount { get; set; }
    }
}
