using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Drawing;
using Wallets.BusinessLayer;




namespace TestWPF
{
    public class UserTest
    {

        [Fact]
        public void ValidateValid()
        {
            //Arrange
            Client valid = new Client(Guid.NewGuid(),"Anton", "Bounce", "rafinazi@gmail.com","bounce");

            //Act
            var actual = valid.Validate();

            //Assert
            Assert.True(actual);

        }

      

        [Fact]
        public void ValidateBadEmail()
        {
            //Arrange
            Client invalid = new Client(Guid.NewGuid(), "Anton", "Bounce", " ", "bounce");

            //Act
            var actual = invalid.Validate();

            //Assert
            Assert.False(actual);

        }

        [Fact]
        public void ValidateBadLogin()
        {
            //Arrange
            Client invalid = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "");

            //Act
            var actual = invalid.Validate();

            //Assert
            Assert.False(actual);

        }



        [Fact]
        public void FullNameTest()
        {
            //Arrange
            Client fullnam = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");
            var expected = "Anton, Bounce";

            //Act
            var actual = fullnam.FullName;

            //Assert
            Assert.True(expected == actual);
        }

        [Fact]
        public void ToStringTest()
        {
            //Arrange
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");
            var expected = "Anton, Bounce";

            //Act
            var actual = user.ToString();

            //Assert
            Assert.True(expected == actual);
        }



        [Fact]
        public void AddWalletTest()
        {
            //Arrange
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");
            //Act
            user.AddWallet("Wallet1", 20.00m, "main wallet", Currency.UAH);

            //Assert
            Assert.True(user.Wallets.Count == 1);
            Assert.NotNull(user.Wallets[0]);
        }

        [Fact]
        public void AddCategoryTestValid()
        {
            //Arrange
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");

            //Act
            user.AddCategory("food", "eat smth", Color.White, null);


            //Assert
            Assert.True(user.Categories.Count == 1);
            Assert.NotNull(user.Categories[0]);
        }

        [Fact]
        public void AddCategoryTestAlreadyExists()
        {
            //Arrange
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");

            //Act
            user.AddCategory("food", "eat smth", Color.White, null);
            user.AddCategory("food", "eat smth", Color.Black, null);

            //Assert
            Assert.True(user.Categories.Count == 1);
            Assert.NotNull(user.Categories[0]);
            Assert.True(user.Categories[0].Color == Color.White);
        }

       

     

        [Fact]
        public void AddWalletCategoryTestValid()
        {
            //Arrange
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");
            user.AddWallet("Wallet1", 20.00m, "main wallet", Currency.UAH);
            user.AddCategory("food", "eat smth", Color.White, null);


            //Act
            user.AddWalletCategory(user.Wallets[0], user.Categories[0]);

            //Assert
            Assert.True(user.Wallets[0].Categories[0] == user.Categories[0]);
        }

        [Fact]
        public void AddWalletCategoryTestNotAccessibleForUser()
        {
            //Arrange
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");
            user.AddWallet("Wallet1", 20.00m, "main wallet", Currency.UAH);


            Client user2 = new Client(Guid.NewGuid(), "Galy", "Malbor", "rrarara@gmail.com", "krikirok");
            user2.AddCategory("food", "eat smth", Color.White, null);
            //Act
            user2.AddWalletCategory(user.Wallets[0], user2.Categories[0]);

            //Assert
            Assert.True(user.Wallets[0].Categories.Count==0);
        }

        [Fact]
        public void AddWalletCategoryTestNotTheUsersCategory()
        {
            //Arrange
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");
            user.AddWallet("Wallet1", 20.00m, "main wallet", Currency.UAH);

            Client user2 = new Client(Guid.NewGuid(), "Galy", "Malbor", "rrarara@gmail.com", "krikirok");
            user2.AddCategory("food", "eat smth", Color.White, null);
            //Act
            user.AddWalletCategory(user.Wallets[0], user2.Categories[0]);

            //Assert
            Assert.True(user.Wallets[0].Categories.Count == 0);
        }

        [Fact]
        public void AddWalletCategoryTestAlreadyExists()
        {
            //Arrange
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");
            user.AddWallet("Wallet1", 20.00m, "main wallet", Currency.UAH);
            user.AddCategory("food", "eat smth", Color.White, null);

            //Act
            user.AddWalletCategory(user.Wallets[0], user.Categories[0]);
            user.AddWalletCategory(user.Wallets[0], user.Categories[0]);
            
            //Assert
            Assert.True(user.Categories.Count == 1);
            Assert.NotNull(user.Categories[0]);
            Assert.True(user.Categories[0].Color == Color.White);
        }






    }
}
