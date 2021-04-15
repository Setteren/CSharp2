using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Drawing;
using Wallets.BusinessLayer;




namespace Wallets.Tests
{
    public class UserTest
    {

        //public User(Guid guid, string lastName, string firstName, string email, string login) : this()





        [Fact]
        public void ValidateValid()
        {
            //Arrange
            Client valid = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");

            //Act
            var actual = valid.Validate();

            //Assert
            Assert.True(actual);

        }

        [Fact]
        public void ValidateBadLastName()
        {
            //Arrange
            Client invalid = new Client(Guid.NewGuid(), "", "Bounce", "rafinazi@gmail.com", "bounce");

            //Act
            var actual = invalid.Validate();

            //Assert
            Assert.False(actual);

        }

        [Fact]
        public void ValidateBadEmail()
        {
            //Arrange
            Client invalid = new Client(Guid.NewGuid(), "Anton", "Bounce", "", "bounce");

            //Act
            var actual = invalid.Validate();

            //Assert
            Assert.False(actual);

        }
        [Fact]
        public void AddWalletTest()
        {
            //Arrange
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");
            //Act
            user.AddWallet("myWallet", 20.00m, "for me", Currency.UAH);

            //Assert
            Assert.True(user.Wallets.Count == 1);
            Assert.NotNull(user.Wallets[0]);
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
        public void ToStringTest()
        {
            //Arrange
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce"); ;
            var expected = "Anton, Bounce";

            //Act
            var actual = user.ToString();

            //Assert
            Assert.True(expected == actual);
        }


        [Fact]
        public void FullNameTest()
        {
            //Arrange
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");;
            var expected = "Anton, Bounce";

            //Act
            var actual = user.FullName;

            //Assert
            Assert.True(expected == actual);
        }



        

        [Fact]
        public void AddCategoryTestValid()
        {
            //Arrange
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");

            //Act
            user.AddCategory("cat", "catty", new ColorWrapper(Color.White), null);


            //Assert
            Assert.True(user.Categories.Count == 1);
            Assert.NotNull(user.Categories[0]);
        }

        [Fact]
        public void AddCategoryTestAlreadyExists()
        {
            //Arrange
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");;

            //Act
            user.AddCategory("cat", "catty", new ColorWrapper(Color.White), null);
            user.AddCategory("cat", "catty", new ColorWrapper(Color.Black), null);

            //Assert
            Assert.True(user.Categories.Count == 1);
            Assert.NotNull(user.Categories[0]);
            Assert.True(user.Categories[0].ColorWrapper.Color == Color.White);
        }

        [Fact]
        public void ShareWalletTestAlreadyExists()
        {
            //Arrange

            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");
            user.AddWallet("myWallet", 20.00m, "for me", Currency.UAH);

            Client user2 = new Client(Guid.NewGuid(), "Galy", "Malbor", "rrarara@gmail.com", "krikirok");

            //Act
            user.ShareWallet(user2, user.Wallets[0]);
            user.ShareWallet(user2, user.Wallets[0]);

            //Assert
            Assert.True(user2.WalletsCoOwnered.Count == 1);
            Assert.True(user2.WalletsCoOwnered[0] == user.Wallets[0]);
            Assert.Equal(user.Wallets[0].CoOwnersGuid[0], user2.Guid);
        }

        [Fact]
        public void ShareWalletTestValid()
        {
            //Arrange
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");;
            user.AddWallet("myWallet", 20.00m, "for me", Currency.UAH);


            Client user2 = new Client(Guid.NewGuid(), "Galy", "Malbor", "rrarara@gmail.com", "krikirok");

            //Act
            user.ShareWallet(user2, user.Wallets[0]);

            //Assert
            Assert.True(user2.WalletsCoOwnered.Count == 1);
            Assert.True(user2.WalletsCoOwnered[0] == user.Wallets[0]);
            Assert.Equal(user.Wallets[0].CoOwnersGuid[0], user2.Guid);
        }

        

        [Fact]
        public void ShareWalletTestForYourself()
        {
            //Arrange
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");;
            user.AddWallet("myWallet", 20.00m, "for me", Currency.UAH);


            //Act
            user.ShareWallet(user, user.Wallets[0]);

            //Assert
            Assert.True(user.WalletsCoOwnered.Count == 0);
            Assert.True(user.Wallets[0].CoOwnersGuid.Count == 0);
        }

        [Fact]
        public void AddWalletCategoryTestValid()
        {
            //Arrange
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce"); ;
            user.AddWallet("myWallet", 20.00m, "for me", Currency.UAH);
            user.AddCategory("cat", "catty", new ColorWrapper(Color.White), null);


            //Act
            user.AddWalletCategory(user.Wallets[0], user.Categories[0]);

            //Assert
            Assert.True(user.Wallets[0].Categories[0] == user.Categories[0]);
        }

        [Fact]
        public void AddWalletCategoryTestNotTheUsersCategory()
        {
            //Arrange
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce"); ;
            user.AddWallet("myWallet", 20.00m, "for me", Currency.UAH);


            Client user2 = new Client(Guid.NewGuid(), "Galy", "Malbor", "rrarara@gmail.com", "krikirok"); ;
            user2.AddCategory("cat", "catty", new ColorWrapper(Color.White), null);
            //Act
            user.AddWalletCategory(user.Wallets[0], user2.Categories[0]);

            //Assert
            Assert.True(user.Wallets[0].Categories.Count == 0);
        }

        [Fact]
        public void ShareWalletTestNotAccessibleForUser()
        {
            //Arrange
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");
            user.AddWallet("myWallet", 20.00m, "for me", Currency.UAH);


             Client user2 = new Client(Guid.NewGuid(), "Galy", "Malbor", "rrarara@gmail.com", "krikirok");
             Client user3 = new Client(Guid.NewGuid(),"Andy", "Borb", "manhattan@gmail.com","birbieeer");
            //Act
            user2.ShareWallet(user3, user.Wallets[0]);
           
            //Assert
            Assert.True(user3.WalletsCoOwnered.Count == 0);
            Assert.True(user.Wallets[0].CoOwnersGuid.Count == 0);
        }

        /// <summary>
        /// AddWalletCategory Test: 
        /// </summary>
        

        [Fact]
        public void AddWalletCategoryTestNotAccessibleForUser()
        {
            //Arrange
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");;
            user.AddWallet("myWallet", 20.00m, "for me", Currency.UAH);


            Client user2 = new Client(Guid.NewGuid(), "Galy", "Malbor", "rrarara@gmail.com", "krikirok");;
            user2.AddCategory("cat", "catty", new ColorWrapper(Color.White), null);
            //Act
            user2.AddWalletCategory(user.Wallets[0], user2.Categories[0]);

            //Assert
            Assert.True(user.Wallets[0].Categories.Count==0);
        }

        

        [Fact]
        public void AddWalletCategoryTestAlreadyExists()
        {
            //Arrange
            Client user = new Client(Guid.NewGuid(), "Anton", "Bounce", "rafinazi@gmail.com", "bounce");
            user.AddWallet("myWallet", 20.00m, "for me", Currency.UAH);
            user.AddCategory("cat", "catty", new ColorWrapper(Color.White), null);

            //Act
            user.AddWalletCategory(user.Wallets[0], user.Categories[0]);
            user.AddWalletCategory(user.Wallets[0], user.Categories[0]);
            
            //Assert
            Assert.True(user.Categories.Count == 1);
            Assert.NotNull(user.Categories[0]);
            Assert.True(user.Categories[0].ColorWrapper.Color == Color.White);
        }






    }
}
