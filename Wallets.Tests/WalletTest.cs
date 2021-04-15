using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xunit;
using System.Drawing;
using Wallets.BusinessLayer;

namespace Wallets.Tests
{
    public class WalletTest
    {

        [Fact]
        public void ValidateValid()
        {
            //Arrange
            Wallet valid = new Wallet(Guid.NewGuid(), "myWallet", 25.01m, "al", Currency.UAH, 
                new ObservableCollection<Transaction>(),new ObservableCollection<Category>(),new List<Guid>()) ;

            //Act
            var actual = valid.Validate();

            //Assert
            Assert.True(actual);

        }


        [Fact]
        public void ValidateBadName()
        {
            //Arrange
            Wallet invalid = new Wallet(Guid.NewGuid(), "  ", 25.01m, "al", Currency.USD,
                new ObservableCollection<Transaction>(), new ObservableCollection<Category>(),new List<Guid>());

            //Act
            var actual = invalid.Validate();

            //Assert
            Assert.False(actual);

        }

        [Fact]
        public void AddTransactionTestNotAccessibleForUser()
        {
            //Arrange
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");
            Wallet wallet = new Wallet(Guid.NewGuid(), "mainW", 20.00m, "al", Currency.USD,
                new ObservableCollection<Transaction>(), new ObservableCollection<Category>(), new List<Guid>()); //is not the id of Alex

            Category cat = new Category(user.Guid, "business", "description here...", new ColorWrapper(Color.Empty), null, Guid.NewGuid());

            Transaction transaction = new Transaction(wallet.Guid, -19.00m, Currency.UAH, cat, "for living",
                DateTime.Today, Guid.NewGuid());
            //Act
            wallet.AddTransaction(user, transaction);

            //Assert
            Assert.True(wallet.Transactions.Count == 0);
        }


        [Fact]
        public void AddTransactionTestValid()
        {
            //Arrange
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");
            Wallet wallet = new Wallet(user.Guid, "mainW", 20.00m, "al", Currency.USD,
                new ObservableCollection<Transaction>(), new ObservableCollection<Category>(), new List<Guid>());

            Category cat = new Category(user.Guid, "business", "description here...", new ColorWrapper(Color.White), null, Guid.NewGuid());

            Transaction transaction = new Transaction(wallet.Guid, -19.00m, Currency.USD, cat, "for living",
                DateTime.Today, Guid.NewGuid());

            //Act
            wallet.AddTransaction(user, transaction);

            //Assert
            Assert.NotNull(wallet.Transactions[0]);
            Assert.True(wallet.Transactions.Count == 1);
            Assert.True(wallet.Balance == 1.00m);
        }

        [Fact]
        public void ValidateBadStartingBalance()
        {
            //Arrange
            Wallet invalid = new Wallet(Guid.NewGuid(), "mainW", -0.001m, "al", Currency.USD,
                new ObservableCollection<Transaction>(),new ObservableCollection<Category>(),new List<Guid>());

            //Act
            var actual = invalid.Validate();

            //Assert
            Assert.False(actual);

        }

        [Fact]
        public void ValidateBadMainCurrency()
        {
            //Arrange
            Wallet invalid = new Wallet(Guid.NewGuid(), "mainW", 10.00m, "al", null,
                new ObservableCollection<Transaction>(),new ObservableCollection<Category>(),new List<Guid>());

            //Act
            var actual = invalid.Validate();

            //Assert
            Assert.False(actual);
        }

     
        
       

        [Fact]
        public void AddTransactionTestSharingWallets()
        {
            //Arrange
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");
            Wallet wallet = new Wallet(user.Guid, "mainW", 20.00m, "al", Currency.USD,
                new ObservableCollection<Transaction>(),new ObservableCollection<Category>(),new List<Guid>());
            Client user2 = new Client(Guid.NewGuid(), "Galy", "Malbor", "rrarara@gmail.com", "krikirok");
            Category cat = new Category(user.Guid, "business", "description here...", new ColorWrapper(Color.White), null,Guid.NewGuid());

            Transaction transaction = new Transaction(wallet.Guid, -19.00m, Currency.USD, cat, "for living",
                DateTime.Today, Guid.NewGuid());
            //Act
            user.ShareWallet(user2, wallet);
            wallet.AddTransaction(user2, transaction);

            //Assert
            Assert.NotNull(wallet.Transactions[0]);
            Assert.True(wallet.Transactions.Count == 1);
            Assert.True(user2.WalletsCoOwnered[0] == wallet);
            Assert.Equal(wallet.CoOwnersGuid[0], user2.Guid);
            Assert.True(wallet.Balance == 1.00m);

        }


        [Fact]
        public void DeleteTransactionTestValid()
        {
            //Arrange
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");
            Wallet wallet = new Wallet(user.Guid, "mainW", 20.00m, "al", Currency.USD,
                new ObservableCollection<Transaction>(), new ObservableCollection<Category>(), new List<Guid>());

            Category cat = new Category(user.Guid, "business", "description here...", new ColorWrapper(Color.White), null, Guid.NewGuid());

            Transaction transaction1 = new Transaction(wallet.Guid, -10.00m, Currency.UAH, cat, "for living",
                DateTime.Today, Guid.NewGuid());

            Transaction transaction2 = new Transaction(wallet.Guid, -9.00m, Currency.UAH, cat, "for living",
                DateTime.Today, Guid.NewGuid());


            wallet.AddTransaction(user, transaction1);
            wallet.AddTransaction(user, transaction2);

            //Act
            wallet.DeleteTransaction(user, wallet.Transactions[0]);

            //Assert
            Assert.NotNull(wallet.Transactions[0]);
            Assert.True(wallet.Transactions.Count == 1);
            Assert.False(wallet.Balance == 11.00m);
        }



        [Fact]
        public void AddTransactionTestCategoryOfOtherUser()
        {
            //Arrange
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");
            Wallet wallet = new Wallet(user.Guid, "mainW", 20.00m, "al", Currency.USD,
                new ObservableCollection<Transaction>(),new ObservableCollection<Category>(),new List<Guid>()); 

            Category cat = new Category(Guid.NewGuid(), "business", "description here...", new ColorWrapper(Color.Empty), null,Guid.NewGuid()); // is not the id of Alex
           
            Transaction transaction = new Transaction(wallet.Guid, -19.00m, Currency.UAH, cat, "for living",
                DateTime.Today, Guid.NewGuid());
            //Act
            wallet.AddTransaction(user, transaction);

            //Assert
            Assert.True(wallet.Transactions.Count == 0);
        }

       



       

      
       

        [Fact]
        public void DeleteTransactionTestTransactionIsNotInList()
        {
            //Arrange
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");
            Category cat = new Category(user.Guid, "business", "description here...", new ColorWrapper(Color.White), null,Guid.NewGuid());

          

           


            Wallet wallet1 = new Wallet(user.Guid, "mainW", 20.00m, "al", Currency.USD,
                new ObservableCollection<Transaction>(),new ObservableCollection<Category>(),new List<Guid>());
            Transaction transaction1 = new Transaction(wallet1.Guid, -19.00m, Currency.UAH, cat, "for living",
                DateTime.Today, Guid.NewGuid());
            wallet1.AddTransaction(user,  transaction1);

            Wallet wallet2 = new Wallet(user.Guid, "mainW", 20.00m, "al", Currency.USD,
                new ObservableCollection<Transaction>(),new ObservableCollection<Category>(),new List<Guid>());
            
            Transaction transaction2 = new Transaction(wallet2.Guid, -18.00m, Currency.UAH, cat, "for drinking",
                DateTime.Today, Guid.NewGuid());
            wallet2.AddTransaction(user, transaction2);


            //Act
            wallet1.DeleteTransaction(user, wallet2.Transactions[0]);
            // wallet2.Transactions[0] is not in list of wallet1.Transactions


            //Assert
            Assert.NotNull(wallet1.Transactions[0]);
            Assert.True(wallet1.Transactions.Count == 1);
            Assert.False(wallet1.Balance == 1.00m);
        }



    }
}
