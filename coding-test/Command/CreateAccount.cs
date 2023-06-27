using System.ComponentModel.DataAnnotations;

namespace coding_test.Command
{
    public class CreateAccount
    {
        [Required]
        public int ClientNumber { get; set; }

        [Required]
        public string AccountType { get; set; }

        [Required]
        public decimal Balance { get; set; }
    }
}
