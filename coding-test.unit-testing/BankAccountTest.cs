using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Threading.Tasks;
using coding_test.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using coding_test.Command;
using coding_test.Data;
using coding_test.Data.Entities;
using coding_test.Services;

namespace coding_test.unit_testing
{
    [TestFixture]
    public class BankAccountControllerTests
    {
        private ServiceProvider? provider;

        [OneTimeSetUp]
        public void SetupAsync()
        {
            provider = Configuration.Setup.SetupServiceProvider((services) =>
            {
                services
                    .AddTransient<BankAccountController>()
                    .AddTransient<BankOperationsController>();
            });

            var dbContext = provider.GetRequiredService<BankDBContext>();

            dbContext.Customers.Add(
                      new Customer
                      {
                          ClientId = 1,
                          FirstName = "Robert",
                          LastName = "Rodriguez"
                      });

            dbContext.SaveChanges();
        }

        [Test]
        public async Task ValidateOpenNewAccount()
        {
            // act
            var controller = provider?.GetRequiredService<BankAccountController>();
            var dbContext = provider.GetRequiredService<BankDBContext>();

            var payload = new CreateAccount
            {
                ClientNumber = 1,
                AccountType = "Savings",
                Balance = 1
            };

            IActionResult response = await controller.CreateAccount(payload);

            // assert 

            var badRequest = response as BadRequestObjectResult;
            Assert.IsNotNull(badRequest, "Response was not 400");


            var payload2 = new CreateAccount
            {
                ClientNumber = 1,
                AccountType = "Savings",
                Balance = 100.00M
            };

            IActionResult response2 = await controller.CreateAccount(payload2);

            // assert
            var createdResult = response2 as CreatedAtActionResult;
            Assert.IsNotNull(createdResult, "Response was not 201");

            var responseAccount = createdResult?.Value as Account;
            Assert.IsNotNull(responseAccount, "Value returned was an Account");

            var account = dbContext?.Accounts
                .Where(w => w.ClientNumber == 1)
                .FirstOrDefault();

            Assert.IsTrue(responseAccount?.Transactions.Sum(s => s.Amount) == payload2.Balance,
                "Account returned did not contain the proper amount of balance");
        }


        [Test]
        public async Task ValidateWithdrawal90Percent()
        {
            // act
            var service = provider?.GetRequiredService<AccountService>();
            var controller = provider?.GetRequiredService<BankOperationsController>();

            var payload = new CreateAccount
            {
                ClientNumber = 1,
                AccountType = "Savings",
                Balance = 10000
            };

            var account =await service.CreateAccount(payload);
            
            IActionResult response = await controller.Withdrawal(
                new BankTransactionCommand
                {
                    AccountNumber = account.ResponseObject.AccountNumber,
                    Amount = 9001
                });

            var responseObj = response as BadRequestObjectResult;
            Assert.IsTrue(responseObj?.Value != "cannot withdraw 90% of your current balance");


            var badRequest = response as BadRequestObjectResult;
            Assert.IsNotNull(badRequest, "Response was not 400");

        }


        [Test]
        public async Task ValidateWithdrawalMinimum()
        {
            // act
            var service = provider?.GetRequiredService<AccountService>();
            var controller = provider?.GetRequiredService<BankOperationsController>();

            var payload = new CreateAccount
            {
                ClientNumber = 1,
                AccountType = "Savings",
                Balance = 100
            };

            await service.CreateAccount(payload);

            IActionResult response = await controller.Withdrawal(
                new BankTransactionCommand
                {
                    AccountNumber = 1,
                    Amount = 1
                });

            var responseObj = response as BadRequestObjectResult;
            Assert.IsTrue(responseObj?.Value !="cannot complete this operation, $100 is the minimum balance");


            var badRequest = response as BadRequestObjectResult;
            Assert.IsNotNull(badRequest, "Response was not 400");

        }

        [Test]
        public async Task Validate10KDeposit()
        {
            // act
            var service = provider?.GetRequiredService<AccountService>();
            var controller = provider?.GetRequiredService<BankOperationsController>();

            var payload = new CreateAccount
            {
                ClientNumber = 1,
                AccountType = "Savings",
                Balance = 100
            };

            await service.CreateAccount(payload);

            IActionResult response = await controller.MakeDeposit(
                new BankTransactionCommand
                {
                    AccountNumber = 1,
                    Amount = 10001
                });

            var responseObj = response as BadRequestObjectResult;
            Assert.IsTrue(responseObj?.Value != "cannot deposit more than $10,000 in a single transaction");


            var badRequest = response as BadRequestObjectResult;
            Assert.IsNotNull(badRequest, "Response was not 400");

        }
    }
}