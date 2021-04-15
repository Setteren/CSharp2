using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using BusinessLayer;
using System.Drawing;

namespace Tests
{
    public class WalletTest
    {

        [Fact]
        public void ValidateValid()
        {
            //Arrange
            Wallet valid = new Wallet(1, "myWallet", 25.01m, "for me", Currency.Usd) ;

            //Act
            var actual = valid.Validate();

            //Assert
            Assert.True(actual);

        }

        [Fact]
        public void ValidateBadOwnerId()
        {
            //Arrange
            Wallet invalid = new Wallet(-1, "myWallet", 25.01m, "for me", Currency.Usd);

            //Act
            var actual = invalid.Validate();

            //Assert
            Assert.False(actual);

        }

        [Fact]
        public void ValidateBadName()
        {
            //Arrange
            Wallet invalid = new Wallet(1, "  ", 25.01m, "for me", Currency.Usd);

            //Act
            var actual = invalid.Validate();

            //Assert
            Assert.False(actual);

        }

        
        [Fact]
        public void ValidateBadStartingBalance()
        {
            //Arrange
            Wallet invalid = new Wallet(1, "My wallet", -0.001m, "for me", Currency.Usd);

            //Act
            var actual = invalid.Validate();

            //Assert
            Assert.False(actual);

        }

        [Fact]
        public void ValidateBadMainCurrency()
        {
            //Arrange
            Wallet invalid = new Wallet(1, "My wallet", 10.00m, "for me", null);

            //Act
            var actual = invalid.Validate();

            //Assert
            Assert.False(actual);
        }

        /// <summary>
        /// AddTransaction tests:
        /// </summary>
        
        [Fact]
        public void AddTransactionTestValid()
        {
            //Arrange
            User alex = new User("Kotkov", "Alex", "alkot2001@gmail.com");
            Wallet wallet = new Wallet(alex.Id, "My wallet", 20.00m, "for me", Currency.Usd);

            Category cat = new Category(alex.Id, "business", "description here...", Color.White, null);

            //Act
            wallet.AddTransaction(alex, -19.00m, Currency.Usd, cat, "for living",null, DateTime.UtcNow);

            //Assert
            Assert.NotNull(wallet.Transactions[0]);
            Assert.True(wallet.Transactions.Count==1);
            Assert.True(wallet.Balance == 1.00m);
        }

        [Fact]
        public void AddTransactionTestSharingWallets()
        {
            //Arrange
            User alex = new User("Kotkov", "Alex", "alkot2001@gmail.com");
            Wallet wallet = new Wallet(alex.Id, "My wallet", 20.00m, "for me", Currency.Usd);
            User dave = new User("Daver", "Dave", "davedaver@gmail.com");
            Category cat = new Category(alex.Id, "business", "description here...", Color.White, null);

            //Act
            alex.ShareWallet(dave, wallet);
            wallet.AddTransaction(dave, -19.00m, Currency.Usd, cat, "for living", null, DateTime.UtcNow);

            //Assert
            Assert.NotNull(wallet.Transactions[0]);
            Assert.True(wallet.Transactions.Count == 1);
            Assert.True(dave.WalletsCoOwnered[0] == wallet);
            Assert.True(wallet.CoOwnersId[0] == dave.Id);
            Assert.True(wallet.Balance == 1.00m);

        }

        [Fact]
        public void AddTransactionTestDifferentCurrencies()
        {
            //Arrange
            User alex = new User("Kotkov", "Alex", "alkot2001@gmail.com");
            Wallet wallet = new Wallet(alex.Id, "My wallet", 20.00m, "for me", Currency.Usd);

            Category cat = new Category(alex.Id, "business", "description here...", Color.White, null);

            //Act
            wallet.AddTransaction(alex, -10.00m, Currency.Uah, cat, "for living", null, DateTime.UtcNow);

            //Assert
            Assert.NotNull(wallet.Transactions[0]);
            Assert.True(wallet.Transactions.Count == 1);
            Assert.True(Decimal.Round(wallet.Balance,2) == 19.64m); // 10 UAH = 0.36 USD
        
        }


        [Fact]
        public void AddTransactionTestNotAccessibleForUser()
        {
            //Arrange
            User alex = new User("Kotkov", "Alex", "alkot2001@gmail.com");
            Wallet wallet = new Wallet(alex.Id+1, "My wallet", 20.00m, "for me", Currency.Usd); //is not the id of Alex
           
            Category cat = new Category(alex.Id, "business", "description here...", Color.Empty, null);

            //Act
            wallet.AddTransaction(alex, -19.00m, Currency.Usd, cat, "for living",null, DateTime.UtcNow);

            //Assert
            Assert.True(wallet.Transactions.Count == 0);
        }

        [Fact]
        public void AddTransactionTestCategoryOfOtherUser()
        {
            //Arrange
            User alex = new User("Kotkov", "Alex", "alkot2001@gmail.com");
            Wallet wallet = new Wallet(alex.Id, "My wallet", 20.00m, "for me", Currency.Usd); 

            Category cat = new Category(alex.Id + 1, "business", "description here...", Color.Empty, null); // is not the id of Alex
            
            //Act
            wallet.AddTransaction(alex, -19.00m, Currency.Usd, cat, "for living", null, DateTime.UtcNow);

            //Assert
            Assert.True(wallet.Transactions.Count == 0);
        }

        /// <summary>
        /// EditTransaction tests:
        /// </summary>

        [Fact]
        public void EditTransactionTestValid()
        {
            //Arrange
            User alex = new User("Kotkov", "Alex", "alkot2001@gmail.com");
            Wallet wallet = new Wallet(alex.Id, "My wallet", 20.00m, "for me", Currency.Usd);
            Category cat = new Category(alex.Id, "business", "description here...", Color.White, null);
            wallet.AddTransaction(alex, -19.00m, Currency.Usd, cat, "for living", null, DateTime.UtcNow);

            //Act
            wallet.EditTransaction(alex, wallet.Transactions[0], -10.00m, Currency.Uah, cat, null, DateTime.UtcNow);

            //Assert
            Assert.NotNull(wallet.Transactions[0]);
            Assert.True(wallet.Transactions.Count == 1);
            Assert.True(Decimal.Round(wallet.Balance, 2) == 19.64m); // 10 UAH = 0.36 USD
        }

        [Fact]
        public void EditTransactionTestNotAccesibleForUser()
        {
            //Arrange
            User alex = new User("Kotkov", "Alex", "alkot2001@gmail.com");
            Wallet wallet = new Wallet(alex.Id, "My wallet", 20.00m, "for me", Currency.Usd);
            Category cat = new Category(alex.Id, "business", "description here...", Color.White, null);
            wallet.AddTransaction(alex, -19.00m, Currency.Usd, cat, "for living", null, DateTime.UtcNow);

            User dave = new User("Dave", "Daver", "davedaver@gmail.com");


            //Act
            wallet.EditTransaction(dave, wallet.Transactions[0], -10.00m, Currency.Uah, cat, null, DateTime.UtcNow);

            //Assert
            Assert.NotNull(wallet.Transactions[0]);
            Assert.True(wallet.Transactions.Count == 1);
            Assert.True(wallet.Balance == 1.00m) ;
        }

        [Fact]
        public void EditTransactionTestTransactionIsNotInList()
        {
            //Arrange
            User alex = new User("Kotkov", "Alex", "alkot2001@gmail.com");
            Category cat = new Category(alex.Id, "business", "description here...", Color.White, null);

            Wallet wallet1 = new Wallet(alex.Id, "My wallet", 20.00m, "for me", Currency.Usd);
            wallet1.AddTransaction(alex, -19.00m, Currency.Usd, cat, "for living", null, DateTime.UtcNow);

            Wallet wallet2 = new Wallet(alex.Id, "My wallet", 20.00m, "for me", Currency.Usd);
            wallet2.AddTransaction(alex, -18.00m, Currency.Uah, cat, "for drinking", null, DateTime.UtcNow); 


            //Act
            wallet1.EditTransaction(alex, wallet2.Transactions[0], -10.00m, Currency.Uah, cat, null, DateTime.UtcNow);
            // wallet2.Transactions[0] is not in list of wallet1.Transactions


            //Assert
            Assert.NotNull(wallet1.Transactions[0]);
            Assert.True(wallet1.Transactions.Count == 1);
            Assert.True(wallet1.Balance == 1.00m);
        }

        [Fact]
        public void EditTransactionTestNotUserCategory()
        {
            //Arrange
            User alex = new User("Kotkov", "Alex", "alkot2001@gmail.com");
            Category cat1 = new Category(alex.Id, "business", "description here...", Color.White, null);

            Wallet wallet = new Wallet(alex.Id, "My wallet", 20.00m, "for me", Currency.Usd);
            wallet.AddTransaction(alex, -19.00m, Currency.Usd, cat1, "for living", null, DateTime.UtcNow);

            Category cat2 = new Category(alex.Id+1, "business", "description here...", Color.White, null); //not the category of Alex

            //Act
            wallet.EditTransaction(alex, wallet.Transactions[0], -10.00m, Currency.Uah, cat2, null, DateTime.UtcNow);


            //Assert
            Assert.NotNull(wallet.Transactions[0]);
            Assert.True(wallet.Transactions.Count == 1);
            Assert.True(wallet.Balance == 1.00m);
        }

        /// <summary>
        /// DeleteTransaction tests:
        /// </summary>

       

        [Fact]
        public void DeleteTransactionTestValid()
        {
            //Arrange
            User alex = new User("Kotkov", "Alex", "alkot2001@gmail.com");
            Wallet wallet = new Wallet(alex.Id, "My wallet", 20.00m, "for me", Currency.Usd);
            Category cat = new Category(alex.Id, "business", "description here...", Color.White, null);
            wallet.AddTransaction(alex, -10.00m, Currency.Usd, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(alex, -9.00m, Currency.Usd, cat, "for living", null, DateTime.UtcNow);

            //Act
            wallet.DeleteTransaction(alex, wallet.Transactions[0]);

            //Assert
            Assert.NotNull(wallet.Transactions[0]);
            Assert.True(wallet.Transactions.Count == 1);
            Assert.True(wallet.Balance == 11.00m);
        }

        [Fact]
        public void DeleteTransactionTestNotAccesibleForUser()
        {
            //Arrange
            User alex = new User("Kotkov", "Alex", "alkot2001@gmail.com");
            Wallet wallet = new Wallet(alex.Id, "My wallet", 20.00m, "for me", Currency.Usd);
            Category cat = new Category(alex.Id, "business", "description here...", Color.White, null);
            wallet.AddTransaction(alex, -10.00m, Currency.Usd, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(alex, -9.00m, Currency.Usd, cat, "for living", null, DateTime.UtcNow);

            User dave = new User("Daver", "Dave", "davedaver@gmail.com");

            //Act
            wallet.DeleteTransaction(dave, wallet.Transactions[0]);

            //Assert
            Assert.NotNull(wallet.Transactions[0]);
            Assert.NotNull(wallet.Transactions[1]);
            Assert.True(wallet.Transactions.Count == 2);
            Assert.True(wallet.Balance == 1.00m);
        }


        [Fact]
        public void DeleteTransactionTestTransactionIsNotInList()
        {
            //Arrange
            User alex = new User("Kotkov", "Alex", "alkot2001@gmail.com");
            Category cat = new Category(alex.Id, "business", "description here...", Color.White, null);

            Wallet wallet1 = new Wallet(alex.Id, "My wallet", 20.00m, "for me", Currency.Usd);
            wallet1.AddTransaction(alex, -19.00m, Currency.Usd, cat, "for living", null, DateTime.UtcNow);

            Wallet wallet2 = new Wallet(alex.Id, "My wallet", 20.00m, "for me", Currency.Usd);
            wallet2.AddTransaction(alex, -18.00m, Currency.Uah, cat, "for drinking", null, DateTime.UtcNow);


            //Act
            wallet1.DeleteTransaction(alex, wallet2.Transactions[0]);
            // wallet2.Transactions[0] is not in list of wallet1.Transactions


            //Assert
            Assert.NotNull(wallet1.Transactions[0]);
            Assert.True(wallet1.Transactions.Count == 1);
            Assert.True(wallet1.Balance == 1.00m);
        }


        [Fact]
        public void LastMonthIncomeAndExpensesTest()
        {
            //Arrange
            User alex = new User("Kotkov", "Alex", "alkot2001@gmail.com");
            Category cat = new Category(alex.Id, "business", "description here...", Color.White, null);

            Wallet wallet1 = new Wallet(alex.Id, "My wallet", 20.00m, "for me", Currency.Usd);
            wallet1.AddTransaction(alex, 1.00m, Currency.Usd, cat, "for living", null, DateTime.UtcNow);
            wallet1.AddTransaction(alex, 1.00m, Currency.Eur, cat, "for living", null, DateTime.UtcNow);
            wallet1.AddTransaction(alex, 1.00m, Currency.Uah, cat, "for living", null, DateTime.UtcNow);
            wallet1.AddTransaction(alex, 1.00m, Currency.Rub, cat, "for living", null, DateTime.UtcNow);

            wallet1.AddTransaction(alex, -1.00m, Currency.Usd, cat, "for living", null, DateTime.UtcNow);
            wallet1.AddTransaction(alex, -1.00m, Currency.Eur, cat, "for living", null, DateTime.UtcNow);
            wallet1.AddTransaction(alex, -1.00m, Currency.Uah, cat, "for living", null, DateTime.UtcNow);
            wallet1.AddTransaction(alex, -1.00m, Currency.Rub, cat, "for living", null, DateTime.UtcNow);

            //Sum of coefficients :  1 + 1/0.84 + 1/27.75 + 1/74.35 = 1 + 1.19 + 0.036 + 0.013 = 2.239
            
            //Act
            var income = wallet1.LastMonthIncome();
            var expenses = wallet1.LastMonthExpense();

           //Assert
            Assert.True(Math.Abs(income-2.239m)<1);
            Assert.True(Math.Abs(expenses+2.239m)<1);
            Assert.True(wallet1.Balance == 20.00m);
        }

        [Fact]
        public void ShowTenTransactionsTest()
        {
            //Arrange
            User alex = new User("Kotkov", "Alex", "alkot2001@gmail.com");
            Category cat = new Category(alex.Id, "business", "description here...", Color.White, null);

            Wallet wallet = new Wallet(alex.Id, "My wallet", 20.00m, "for me", Currency.Usd);
            wallet.AddTransaction(alex, 1.00m, Currency.Usd, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(alex, 2.00m, Currency.Eur, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(alex, 3.00m, Currency.Uah, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(alex, 4.00m, Currency.Rub, cat, "for living", null, DateTime.UtcNow);

            wallet.AddTransaction(alex, 11.00m, Currency.Usd, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(alex, 12.00m, Currency.Eur, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(alex, 13.00m, Currency.Uah, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(alex, 14.00m, Currency.Rub, cat, "for living", null, DateTime.UtcNow);
            
            wallet.AddTransaction(alex, 21.00m, Currency.Usd, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(alex, 22.00m, Currency.Eur, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(alex, 23.00m, Currency.Uah, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(alex, 24.00m, Currency.Rub, cat, "for living", null, DateTime.UtcNow);

            //Act
            var result = wallet.ShowTenTransactions(1);

            //Assert
            Assert.True(result.Count == 10);
            for(int i = 1; i < 11; i++)
            {
                Assert.True(result[i - 1] == wallet.Transactions[i]);
            }
        }

        [Fact]
        public void ShowTenTransactionsTestNumberPlusTenBiggerThanCount()
        {
            //Arrange
            User alex = new User("Kotkov", "Alex", "alkot2001@gmail.com");
            Category cat = new Category(alex.Id, "business", "description here...", Color.White, null);

            Wallet wallet = new Wallet(alex.Id, "My wallet", 20.00m, "for me", Currency.Usd);
            wallet.AddTransaction(alex, 1.00m, Currency.Usd, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(alex, 2.00m, Currency.Eur, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(alex, 3.00m, Currency.Uah, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(alex, 4.00m, Currency.Rub, cat, "for living", null, DateTime.UtcNow);

            wallet.AddTransaction(alex, 11.00m, Currency.Usd, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(alex, 12.00m, Currency.Eur, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(alex, 13.00m, Currency.Uah, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(alex, 14.00m, Currency.Rub, cat, "for living", null, DateTime.UtcNow);

            wallet.AddTransaction(alex, 21.00m, Currency.Usd, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(alex, 22.00m, Currency.Eur, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(alex, 23.00m, Currency.Uah, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(alex, 24.00m, Currency.Rub, cat, "for living", null, DateTime.UtcNow);

            //Act
            var result = wallet.ShowTenTransactions(5);

            //Assert
            Assert.True(result.Count == 10);
            for (int i = 2; i < 12; i++)
            {
                Assert.True(result[i - 2] == wallet.Transactions[i]);
            }
        }



        [Fact]
        public void ShowTenTransactionsTestTenBiggerThanCount()
        {
            //Arrange
            User alex = new User("Kotkov", "Alex", "alkot2001@gmail.com");
            Category cat = new Category(alex.Id, "business", "description here...", Color.White, null);

            Wallet wallet = new Wallet(alex.Id, "My wallet", 20.00m, "for me", Currency.Usd);
            wallet.AddTransaction(alex, 1.00m, Currency.Usd, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(alex, 2.00m, Currency.Eur, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(alex, 3.00m, Currency.Uah, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(alex, 4.00m, Currency.Rub, cat, "for living", null, DateTime.UtcNow);

            wallet.AddTransaction(alex, 11.00m, Currency.Usd, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(alex, 12.00m, Currency.Eur, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(alex, 13.00m, Currency.Uah, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(alex, 14.00m, Currency.Rub, cat, "for living", null, DateTime.UtcNow);

            //Act
            var result = wallet.ShowTenTransactions(0);

            //Assert
            Assert.True(result.Count == wallet.Transactions.Count);
            for (int i = 0; i < result.Count; i++)
            {
                Assert.True(result[i] == wallet.Transactions[i]);
            }
        }

    }
}
