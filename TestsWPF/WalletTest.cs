using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Drawing;
using Wallets.BusinessLayer;

namespace TestWPF
{
    public class WalletTest
    {

        [Fact]
        public void ValidateValid()
        {
            //Arrange
            Wallet valid = new Wallet(Guid.NewGuid(), "Wallet1", 25.01m, "main wallet", Currency.UAH) ;

            //Act
            var actual = valid.Validate();

            //Assert
            Assert.True(actual);

        }


        [Fact]
        public void ValidateBadName()
        {
            //Arrange
            Wallet invalid = new Wallet(Guid.NewGuid(), "  ", 25.01m, "main wallet", Currency.USD);

            //Act
            var actual = invalid.Validate();

            //Assert
            Assert.False(actual);

        }

        
        [Fact]
        public void ValidateBadStartingBalance()
        {
            //Arrange
            Wallet invalid = new Wallet(Guid.NewGuid(), "Wallet1", -0.001m, "main wallet", Currency.USD);

            //Act
            var actual = invalid.Validate();

            //Assert
            Assert.False(actual);

        }

        [Fact]
        public void ValidateBadMainCurrency()
        {
            //Arrange
            Wallet invalid = new Wallet(Guid.NewGuid(), "Wallet1", 10.00m, "main wallet", null);

            //Act
            var actual = invalid.Validate();

            //Assert
            Assert.False(actual);
        }


        
        [Fact]
        public void AddTransactionTestValid()
        {
            //Arrange
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");
            Wallet wallet = new Wallet(user.Guid, "My wallet", 20.00m, "for me", Currency.USD);

            Category cat = new Category(user.Guid, "business", "description here...", Color.White, null);

            //Act
            wallet.AddTransaction(user, -19.00m, Currency.USD, cat, "for living",null, DateTime.UtcNow);

            //Assert
            Assert.NotNull(wallet.Transactions[0]);
            Assert.True(wallet.Transactions.Count==1);
            Assert.True(wallet.Balance == 1.00m);
        }

        [Fact]
        public void AddTransactionTestSharingWallets()
        {
            //Arrange
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");
            Wallet wallet = new Wallet(user.Guid, "My wallet", 20.00m, "for me", Currency.USD);
            Client user2 = new Client(Guid.NewGuid(), "Galy", "Malbor", "rrarara@gmail.com", "krikirok");
            Category cat = new Category(user.Guid, "business", "description here...", Color.White, null);

            //Act
            user.ShareWallet(user2, wallet);
            wallet.AddTransaction(user2, -19.00m, Currency.USD, cat, "for living", null, DateTime.UtcNow);

            //Assert
            Assert.NotNull(wallet.Transactions[0]);
            Assert.True(wallet.Transactions.Count == 1);
            Assert.True(user2.WalletsCoOwnered[0] == wallet);
            Assert.Equal(wallet.CoOwnersGuid[0], user2.Guid);
            Assert.True(wallet.Balance == 1.00m);

        }

        [Fact]
        public void AddTransactionTestDifferentCurrencies()
        {
            //Arrange
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");
            Wallet wallet = new Wallet(user.Guid, "My wallet", 20.00m, "for me", Currency.USD);

            Category cat = new Category(user.Guid, "business", "description here...", Color.White, null);

            //Act
            wallet.AddTransaction(user, -10.00m, Currency.UAH, cat, "for living", null, DateTime.UtcNow);

            //Assert
            Assert.NotNull(wallet.Transactions[0]);
            Assert.True(wallet.Transactions.Count == 1);
            Assert.True(Decimal.Round(wallet.Balance,2) == 19.64m); // 10 UAH = 0.36 USD
        
        }


        [Fact]
        public void AddTransactionTestNotAccessibleForUser()
        {
            //Arrange
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");
            Wallet wallet = new Wallet(Guid.NewGuid(), "My wallet", 20.00m, "for me", Currency.USD); //is not the id of Alex
           
            Category cat = new Category(user.Guid, "business", "description here...", Color.Empty, null);

            //Act
            wallet.AddTransaction(user, -19.00m, Currency.USD, cat, "for living",null, DateTime.UtcNow);

            //Assert
            Assert.True(wallet.Transactions.Count == 0);
        }

        [Fact]
        public void AddTransactionTestCategoryOfOtherUser()
        {
            //Arrange
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");
            Wallet wallet = new Wallet(user.Guid, "My wallet", 20.00m, "for me", Currency.USD); 

            Category cat = new Category(Guid.NewGuid(), "business", "description here...", Color.Empty, null); // is not the id of Alex
            
            //Act
            wallet.AddTransaction(user, -19.00m, Currency.USD, cat, "for living", null, DateTime.UtcNow);

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
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");
            Wallet wallet = new Wallet(user.Guid, "My wallet", 20.00m, "for me", Currency.USD);
            Category cat = new Category(user.Guid, "business", "description here...", Color.White, null);
            wallet.AddTransaction(user, -19.00m, Currency.USD, cat, "for living", null, DateTime.UtcNow);

            //Act
            wallet.EditTransaction(user, wallet.Transactions[0], -10.00m, Currency.UAH, cat, null, DateTime.UtcNow);

            //Assert
            Assert.NotNull(wallet.Transactions[0]);
            Assert.True(wallet.Transactions.Count == 1);
            Assert.True(Decimal.Round(wallet.Balance, 2) == 19.64m); // 10 UAH = 0.36 USD
        }

        [Fact]
        public void EditTransactionTestNotAccesibleForUser()
        {
            //Arrange
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");
            Wallet wallet = new Wallet(user.Guid, "My wallet", 20.00m, "for me", Currency.USD);
            Category cat = new Category(user.Guid, "business", "description here...", Color.White, null);
            wallet.AddTransaction(user, -19.00m, Currency.USD, cat, "for living", null, DateTime.UtcNow);

            Client user2 = new Client(Guid.NewGuid(), "Galy", "Malbor", "rrarara@gmail.com", "krikirok");


            //Act
            wallet.EditTransaction(user2, wallet.Transactions[0], -10.00m, Currency.UAH, cat, null, DateTime.UtcNow);

            //Assert
            Assert.NotNull(wallet.Transactions[0]);
            Assert.True(wallet.Transactions.Count == 1);
            Assert.True(wallet.Balance == 1.00m) ;
        }

        [Fact]
        public void EditTransactionTestTransactionIsNotInList()
        {
            //Arrange
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");
            Category cat = new Category(user.Guid, "business", "description here...", Color.White, null);

            Wallet wallet1 = new Wallet(user.Guid, "My wallet", 20.00m, "for me", Currency.USD);
            wallet1.AddTransaction(user, -19.00m, Currency.USD, cat, "for living", null, DateTime.UtcNow);

            Wallet wallet2 = new Wallet(user.Guid, "My wallet", 20.00m, "for me", Currency.USD);
            wallet2.AddTransaction(user, -18.00m, Currency.UAH, cat, "for drinking", null, DateTime.UtcNow); 


            //Act
            wallet1.EditTransaction(user, wallet2.Transactions[0], -10.00m, Currency.UAH, cat, null, DateTime.UtcNow);
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
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");
            Category cat1 = new Category(user.Guid, "business", "description here...", Color.White, null);

            Wallet wallet = new Wallet(user.Guid, "My wallet", 20.00m, "for me", Currency.USD);
            wallet.AddTransaction(user, -19.00m, Currency.USD, cat1, "for living", null, DateTime.UtcNow);

            Category cat2 = new Category(Guid.NewGuid(), "business", "description here...", Color.White, null); //not the category of Alex

            //Act
            wallet.EditTransaction(user, wallet.Transactions[0], -10.00m, Currency.UAH, cat2, null, DateTime.UtcNow);


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
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");
            Wallet wallet = new Wallet(user.Guid, "My wallet", 20.00m, "for me", Currency.USD);
            Category cat = new Category(user.Guid, "business", "description here...", Color.White, null);
            wallet.AddTransaction(user, -10.00m, Currency.USD, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(user, -9.00m, Currency.USD, cat, "for living", null, DateTime.UtcNow);

            //Act
            wallet.DeleteTransaction(user, wallet.Transactions[0]);

            //Assert
            Assert.NotNull(wallet.Transactions[0]);
            Assert.True(wallet.Transactions.Count == 1);
            Assert.True(wallet.Balance == 11.00m);
        }

        [Fact]
        public void DeleteTransactionTestNotAccesibleForUser()
        {
            //Arrange
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");
            Wallet wallet = new Wallet(user.Guid, "My wallet", 20.00m, "for me", Currency.USD);
            Category cat = new Category(user.Guid, "business", "description here...", Color.White, null);
            wallet.AddTransaction(user, -10.00m, Currency.USD, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(user, -9.00m, Currency.USD, cat, "for living", null, DateTime.UtcNow);

            Client user2 = new Client(Guid.NewGuid(), "Galy", "Malbor", "rrarara@gmail.com", "krikirok");

            //Act
            wallet.DeleteTransaction(user2, wallet.Transactions[0]);

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
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");
            Category cat = new Category(user.Guid, "business", "description here...", Color.White, null);

            Wallet wallet1 = new Wallet(user.Guid, "My wallet", 20.00m, "for me", Currency.USD);
            wallet1.AddTransaction(user, -19.00m, Currency.USD, cat, "for living", null, DateTime.UtcNow);

            Wallet wallet2 = new Wallet(user.Guid, "My wallet", 20.00m, "for me", Currency.USD);
            wallet2.AddTransaction(user, -18.00m, Currency.UAH, cat, "for drinking", null, DateTime.UtcNow);


            //Act
            wallet1.DeleteTransaction(user, wallet2.Transactions[0]);
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
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");
            Category cat = new Category(user.Guid, "business", "description here...", Color.White, null);

            Wallet wallet1 = new Wallet(user.Guid, "My wallet", 20.00m, "for me", Currency.USD);
            wallet1.AddTransaction(user, 1.00m, Currency.USD, cat, "for living", null, DateTime.UtcNow);
            wallet1.AddTransaction(user, 1.00m, Currency.EUR, cat, "for living", null, DateTime.UtcNow);
            wallet1.AddTransaction(user, 1.00m, Currency.UAH, cat, "for living", null, DateTime.UtcNow);
            wallet1.AddTransaction(user, 1.00m, Currency.RUB, cat, "for living", null, DateTime.UtcNow);

            wallet1.AddTransaction(user, -1.00m, Currency.USD, cat, "for living", null, DateTime.UtcNow);
            wallet1.AddTransaction(user, -1.00m, Currency.EUR, cat, "for living", null, DateTime.UtcNow);
            wallet1.AddTransaction(user, -1.00m, Currency.UAH, cat, "for living", null, DateTime.UtcNow);
            wallet1.AddTransaction(user, -1.00m, Currency.RUB, cat, "for living", null, DateTime.UtcNow);

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
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");
            Category cat = new Category(user.Guid, "business", "description here...", Color.White, null);

            Wallet wallet = new Wallet(user.Guid, "My wallet", 20.00m, "for me", Currency.USD);
            wallet.AddTransaction(user, 1.00m, Currency.USD, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(user, 2.00m, Currency.EUR, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(user, 3.00m, Currency.UAH, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(user, 4.00m, Currency.RUB, cat, "for living", null, DateTime.UtcNow);

            wallet.AddTransaction(user, 11.00m, Currency.USD, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(user, 12.00m, Currency.EUR, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(user, 13.00m, Currency.UAH, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(user, 14.00m, Currency.RUB, cat, "for living", null, DateTime.UtcNow);
            
            wallet.AddTransaction(user, 21.00m, Currency.USD, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(user, 22.00m, Currency.EUR, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(user, 23.00m, Currency.UAH, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(user, 24.00m, Currency.RUB, cat, "for living", null, DateTime.UtcNow);

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
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");
            Category cat = new Category(user.Guid, "business", "description here...", Color.White, null);

            Wallet wallet = new Wallet(user.Guid, "My wallet", 20.00m, "for me", Currency.USD);
            wallet.AddTransaction(user, 1.00m, Currency.USD, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(user, 2.00m, Currency.EUR, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(user, 3.00m, Currency.UAH, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(user, 4.00m, Currency.RUB, cat, "for living", null, DateTime.UtcNow);

            wallet.AddTransaction(user, 11.00m, Currency.USD, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(user, 12.00m, Currency.EUR, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(user, 13.00m, Currency.UAH, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(user, 14.00m, Currency.RUB, cat, "for living", null, DateTime.UtcNow);

            wallet.AddTransaction(user, 21.00m, Currency.USD, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(user, 22.00m, Currency.EUR, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(user, 23.00m, Currency.UAH, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(user, 24.00m, Currency.RUB, cat, "for living", null, DateTime.UtcNow);

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
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");
            Category cat = new Category(user.Guid, "business", "description here...", Color.White, null);

            Wallet wallet = new Wallet(user.Guid, "My wallet", 20.00m, "for me", Currency.USD);
            wallet.AddTransaction(user, 1.00m, Currency.USD, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(user, 2.00m, Currency.EUR, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(user, 3.00m, Currency.UAH, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(user, 4.00m, Currency.RUB, cat, "for living", null, DateTime.UtcNow);

            wallet.AddTransaction(user, 11.00m, Currency.USD, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(user, 12.00m, Currency.EUR, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(user, 13.00m, Currency.UAH, cat, "for living", null, DateTime.UtcNow);
            wallet.AddTransaction(user, 14.00m, Currency.RUB, cat, "for living", null, DateTime.UtcNow);

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
