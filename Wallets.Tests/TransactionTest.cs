using System;
using System.Drawing;
using Wallets.BusinessLayer;
using Xunit;

namespace Wallets.Tests
{
    public class TransactionTest
    {
        [Fact]
        public void ValidateValid()
        {
            //Arrange
            Category validCat = new Category(Guid.NewGuid(), "business", "description here...", new ColorWrapper(System.Drawing.Color.White), null, Guid.NewGuid());


            Transaction valid = new Transaction(Guid.NewGuid(), -85.55m, Currency.USD, validCat,"nothing important",System.DateTime.Now,Guid.NewGuid());

            //Act
            var actual = valid.Validate();

            //Assert
            Assert.True(actual);
        }

        [Fact]
        public void ValidateBadCurrency()
        {
            //Arrange
            Category validCat = new Category(Guid.NewGuid(), "business", "description here...", new ColorWrapper(System.Drawing.Color.White), null, Guid.NewGuid());
            Transaction invalid = new Transaction(Guid.NewGuid(), -85.55m, null, validCat, "nothing important", System.DateTime.Now, Guid.NewGuid());

            //Act
            var actual = invalid.Validate();

            //Assert
            Assert.False(actual);
        }

        [Fact]
        public void ValidateBadMoneyAmount()
        {
            //Arrange
            Category validCat = new Category(Guid.NewGuid(), "business", "description here...", new ColorWrapper(System.Drawing.Color.White), null, Guid.NewGuid());

            Transaction invalid = new Transaction(Guid.NewGuid(), 0.0m, Currency.USD, null, "nothing important", System.DateTime.Now, Guid.NewGuid());

            //Act
            var actual = invalid.Validate();

            //Assert
            Assert.False(actual);
        }


        [Fact]
        public void ValidateBadCategory()
        {
            //Arrange
            Transaction invalid = new Transaction(Guid.NewGuid(), -85.55m, Currency.USD, null, "nothing important", System.DateTime.Now, Guid.NewGuid());

            //Act
            var actual = invalid.Validate();

            //Assert
            Assert.False(actual);
        }

       

       


     


    }
}
