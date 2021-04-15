using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using BusinessLayer;
using System.Drawing;

namespace Tests
{
    public class UserTest
    {
        [Fact]
        public void ValidateValid()
        {
            //Arrange
            User valid = new User("Kotkov", "Alex", "alkot2001@gmail.com");

            //Act
            var actual = valid.Validate();

            //Assert
            Assert.True(actual);

        }

        [Fact]
        public void ValidateBadLastName()
        {
            //Arrange
            User invalid = new User(" ", "Alex", "alkot2001@gmail.com");

            //Act
            var actual = invalid.Validate();

            //Assert
            Assert.False(actual);

        }

        [Fact]
        public void ValidateBadEmail()
        {
            //Arrange
            User invalid = new User("Kotkov", "Alex", " ");

            //Act
            var actual = invalid.Validate();

            //Assert
            Assert.False(actual);

        }

        [Fact]
        public void FullNameTest()
        {
            //Arrange
            User alex = new User("Kotkov", "Alex", "alkot2001@gmail.com");
            var expected = "Kotkov, Alex";

            //Act
            var actual = alex.FullName;

            //Assert
            Assert.True(expected == actual);
        }

        [Fact]
        public void ToStringTest()
        {
            //Arrange
            User alex = new User("Kotkov", "Alex", "alkot2001@gmail.com");
            var expected = "Kotkov, Alex";

            //Act
            var actual = alex.FullName;

            //Assert
            Assert.True(expected == actual);
        }

        [Fact]
        public void AddWalletTest()
        {
            //Arrange
            User alex = new User("Kotkov", "Alex", "alkot2001@gmail.com");

            //Act
            alex.AddWallet("myWallet", 20.00m, "for me", Currency.Uah);


            //Assert
            Assert.True(alex.Wallets.Count == 1);
            Assert.NotNull(alex.Wallets[0]);
        }

        [Fact]
        public void AddCategoryTestValid()
        {
            //Arrange
            User alex = new User("Kotkov", "Alex", "alkot2001@gmail.com");

            //Act
            alex.AddCategory("cat", "catty", Color.White, null);


            //Assert
            Assert.True(alex.Categories.Count == 1);
            Assert.NotNull(alex.Categories[0]);
        }

        [Fact]
        public void AddCategoryTestAlreadyExists()
        {
            //Arrange
            User alex = new User("Kotkov", "Alex", "alkot2001@gmail.com");

            //Act
            alex.AddCategory("cat", "catty", Color.White, null);
            alex.AddCategory("cat", "catty", Color.Black, null);

            //Assert
            Assert.True(alex.Categories.Count == 1);
            Assert.NotNull(alex.Categories[0]);
            Assert.True(alex.Categories[0].Color == Color.White);
        }

        [Fact]
        public void ShareWalletTestValid()
        {
            //Arrange
            User alex = new User("Kotkov", "Alex", "alkot2001@gmail.com");
            alex.AddWallet("myWallet", 20.00m, "for me", Currency.Uah);


            User dave = new User("Daver", "Dave", "daverdave@gmail.com");

            //Act
            alex.ShareWallet(dave, alex.Wallets[0]);

            //Assert
            Assert.True(dave.WalletsCoOwnered.Count == 1);
            Assert.True(dave.WalletsCoOwnered[0] == alex.Wallets[0]);
            Assert.True(alex.Wallets[0].CoOwnersId[0] == dave.Id);
        }

        [Fact]
        public void ShareWalletTestAlreadyExists()
        {
            //Arrange
            User alex = new User("Kotkov", "Alex", "alkot2001@gmail.com");
            alex.AddWallet("myWallet", 20.00m, "for me", Currency.Uah);


            User dave = new User("Daver", "Dave", "daverdave@gmail.com");

            //Act
            alex.ShareWallet(dave, alex.Wallets[0]);
            alex.ShareWallet(dave, alex.Wallets[0]);

            //Assert
            Assert.True(dave.WalletsCoOwnered.Count == 1);
            Assert.True(dave.WalletsCoOwnered[0] == alex.Wallets[0]);
            Assert.True(alex.Wallets[0].CoOwnersId[0] == dave.Id);
        }

        [Fact]
        public void ShareWalletTestForYourself()
        {
            //Arrange
            User alex = new User("Kotkov", "Alex", "alkot2001@gmail.com");
            alex.AddWallet("myWallet", 20.00m, "for me", Currency.Uah);


            //Act
            alex.ShareWallet(alex, alex.Wallets[0]);

            //Assert
            Assert.True(alex.WalletsCoOwnered.Count == 0);
            Assert.True(alex.Wallets[0].CoOwnersId.Count == 0);
        }

        [Fact]
        public void ShareWalletTestNotAccessibleForUser()
        {
            //Arrange
            User alex = new User("Kotkov", "Alex", "alkot2001@gmail.com");
            alex.AddWallet("myWallet", 20.00m, "for me", Currency.Uah);


            User dave = new User("Daver", "Dave", "daverdave@gmail.com");
            User karl = new User("Karl", "Karling", "lovebeer@gmail.com");
            //Act
            dave.ShareWallet(karl, alex.Wallets[0]);
           
            //Assert
            Assert.True(karl.WalletsCoOwnered.Count == 0);
            Assert.True(alex.Wallets[0].CoOwnersId.Count == 0);
        }

        /// <summary>
        /// AddWalletCategory Test: 
        /// </summary>
        [Fact]
        public void AddWalletCategoryTestValid()
        {
            //Arrange
            User alex = new User("Kotkov", "Alex", "alkot2001@gmail.com");
            alex.AddWallet("myWallet", 20.00m, "for me", Currency.Uah);
            alex.AddCategory("cat", "catty", Color.White, null);


            //Act
            alex.AddWalletCategory(alex.Wallets[0], alex.Categories[0]);

            //Assert
            Assert.True(alex.Wallets[0].Categories[0] == alex.Categories[0]);
        }

        [Fact]
        public void AddWalletCategoryTestNotAccessibleForUser()
        {
            //Arrange
            User alex = new User("Kotkov", "Alex", "alkot2001@gmail.com");
            alex.AddWallet("myWallet", 20.00m, "for me", Currency.Uah);
           

            User dave = new User("Daver", "Dave", "daverdave@gmail.com");
            dave.AddCategory("cat", "catty", Color.White, null);
            //Act
            dave.AddWalletCategory(alex.Wallets[0], dave.Categories[0]);

            //Assert
            Assert.True(alex.Wallets[0].Categories.Count==0);
        }

        [Fact]
        public void AddWalletCategoryTestNotTheUsersCategory()
        {
            //Arrange
            User alex = new User("Kotkov", "Alex", "alkot2001@gmail.com");
            alex.AddWallet("myWallet", 20.00m, "for me", Currency.Uah);


            User dave = new User("Daver", "Dave", "daverdave@gmail.com");
            dave.AddCategory("cat", "catty", Color.White, null);
            //Act
            alex.AddWalletCategory(alex.Wallets[0], dave.Categories[0]);

            //Assert
            Assert.True(alex.Wallets[0].Categories.Count == 0);
        }

        [Fact]
        public void AddWalletCategoryTestAlreadyExists()
        {
            //Arrange
            User alex = new User("Kotkov", "Alex", "alkot2001@gmail.com");
            alex.AddWallet("myWallet", 20.00m, "for me", Currency.Uah);
            alex.AddCategory("cat", "catty", Color.White, null);

            //Act
            alex.AddWalletCategory(alex.Wallets[0], alex.Categories[0]);
            alex.AddWalletCategory(alex.Wallets[0], alex.Categories[0]);
            
            //Assert
            Assert.True(alex.Categories.Count == 1);
            Assert.NotNull(alex.Categories[0]);
            Assert.True(alex.Categories[0].Color == Color.White);
        }

    }
}
