using coding_test.Command;
using coding_test.Data.Entities;
using coding_test.Repositories;
using Microsoft.EntityFrameworkCore;

namespace coding_test.Services
{
    public class AccountService
    {
        private readonly IRepository<Account> accountRepository;
        private readonly IRepository<Customer> customerRepository;
        private readonly IRepository<Transaction> transactionRepository;
        private readonly IUnitOfWork unitOfWork;

        public AccountService(
            IUnitOfWork unitOfWork,
            IRepository<Account> accountRepository,
            IRepository<Customer> customerRepository,
            IRepository<Transaction> transactionRepository
            )
        {
            this.accountRepository = accountRepository;
            this.customerRepository = customerRepository;
            this.transactionRepository = transactionRepository;
            this.unitOfWork = unitOfWork;
        }


        public async Task<Customer> GetCustomer(int clientNumber)
        {
            var customer = customerRepository.GetById(clientNumber);

            return await Task.FromResult(customer);
        }

        public async Task<IEnumerable<Account>> GetAccounts(int clientNumber)
        {
            var accounts = await accountRepository.GetAll()
                                             .Where(w => w.ClientNumber == clientNumber)
                                             .Include(w => w.Transactions)
                                             .ToListAsync();

            return accounts;
        }

        public async Task<ServiceResponse<bool>> RemoveAccount(int accountNumber)
        {
            try
            {
                var account = accountRepository.GetById(accountNumber);

                accountRepository.Remove(account);

                unitOfWork.Commit();

                return await Task.FromResult(new ServiceResponse<bool>(true));

            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();

                return new ServiceResponse<bool>(new Exception("could not create account"));
            }
        }

        public async Task<ServiceResponse<Account>> CreateAccount(CreateAccount command)
        {
            try
            {
                if (command.Balance < 100) return new ServiceResponse<Account>(new Exception("cannot create account with less than $100"));

                var customer = customerRepository.GetById(command.ClientNumber);

                var newAccount = new Account
                {
                    Customer = customer,
                    AccountNumber = await GetAccountNumber(),
                    AccountType = command.AccountType,
                    ClientNumber = command.ClientNumber
                };

                transactionRepository.Add(new Transaction
                {
                    Account = newAccount,
                    AddedOn = DateTime.Now,
                    CreatedBy = $"{customer.FirstName}, {customer.LastName}",
                    Amount = command.Balance,
                    AccountNumber = newAccount.AccountNumber,
                    TransactionId = transactionRepository.GetAll().Count() + 1
                });


                unitOfWork.Commit();

                return new ServiceResponse<Account>(newAccount);

            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                return new ServiceResponse<Account>(new Exception("could not create account"));
            }

            async Task<int> GetAccountNumber()
            {
                while (true)
                {
                    var newNumber = new Random().Next(1, int.MaxValue);

                    if (!await accountRepository.GetAll().AnyAsync(a => a.AccountNumber.Equals(newNumber)))
                        return newNumber;
                }
            }
        }
    }
}
