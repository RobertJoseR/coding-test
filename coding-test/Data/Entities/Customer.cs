using System.ComponentModel.DataAnnotations;

namespace coding_test.Data.Entities
{
    public class Customer
    {
        [Key]
        public int ClientId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<Account> Accounts { get; set; }

    }
}
