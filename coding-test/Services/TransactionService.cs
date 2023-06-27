using coding_test.Command;
using coding_test.Data.Entities;
using coding_test.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace coding_test.Services
{
    public class TransactionService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepository<Account> accountRepository;
        private readonly IRepository<Customer> customerRepository;
        private readonly IRepository<Transaction> transactionRepository;

        public TransactionService(
            IUnitOfWork unitOfWork,
            IRepository<Account> accountRepository,
            IRepository<Customer> customerRepository,
            IRepository<Transaction> transactionRepository
            )
        {
            this.unitOfWork = unitOfWork;
            this.accountRepository = accountRepository;
            this.customerRepository = customerRepository;
            this.transactionRepository = transactionRepository;
        }

        public async Task<ServiceResponse<Account>> MakeDeposit(BankTransactionCommand command)
        {
            // Validate / Execute
            return command.Amount > 10000
                  ? new ServiceResponse<Account>(
                      new Exception("cannot deposit more than $10,000 in a single transaction")
                      ) 
                  : await CreateTransaction(command);
        }

        public async Task<ServiceResponse<Account>> WithDrawal(BankTransactionCommand command)
        {
            var balance = await transactionRepository.GetAll()
                                                  .Where(w => w.AccountNumber == command.AccountNumber)
                                                  .SumAsync(s => s.Amount);

            // Validations
            if(balance - command.Amount < 100)
            {
                return new ServiceResponse<Account>(
                    new Exception("cannot complete this operation, $100 is the minimum balance")
                    );
            }
            if (command.Amount > (balance * 0.9M))
            {
                return new ServiceResponse<Account>(
                    new Exception("cannot withdraw 90% of your current balance")
                    );
            }

            // Execute Withdrawal
            command.Amount *= -1;
            return await CreateTransaction(command);
        }


        private async Task<ServiceResponse<Account>> CreateTransaction(BankTransactionCommand command)
        {
            try
            {
                var details = await accountRepository.GetAll()
                                                        .Where(w => w.AccountNumber == command.AccountNumber)
                                                        .Join(customerRepository.GetAll(),
                                                           acc => acc.ClientNumber,
                                                           cust => cust.ClientId,
                                                           (acc, cust) => new
                                                           {
                                                               Name = $"{cust.FirstName}, {cust.LastName}",
                                                               Account = acc
                                                           })
                                                        .FirstOrDefaultAsync();

                transactionRepository.Add(new Transaction
                {
                    Account = details.Account,
                    AddedOn = DateTime.Now,
                    CreatedBy = details.Name,
                    Amount = command.Amount,
                    AccountNumber = command.AccountNumber,
                    TransactionId = transactionRepository.GetAll().Count() + 1
                });

                unitOfWork.Commit();

                details.Account.Transactions = await transactionRepository.GetAll()
                                                                           .Where(w => w.AccountNumber == command.AccountNumber)
                                                                           .ToListAsync();

                return new ServiceResponse<Account>(details.Account);

            }
            catch (Exception ex)
            {
                unitOfWork.Rollback();
                throw new Exception("cannot create transaction");
            }

        }
    }
}
