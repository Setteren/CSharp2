using System;
using System.Drawing;
using Wallets.BusinessLayer;
using Xunit;

namespace TestWPF
{
    public class TransactionTest
    {
        [Fact]
        public void ValidateValid()
        {
            //Arrange
            Category validCat = new Category(Guid.NewGuid(), "business", "description here...",Color.Empty,null);

            Transaction valid = new Transaction(Guid.NewGuid(), -85.55m, Currency.USD, validCat,"nothing important",null, System.DateTime.Now);

            //Act
            var actual = valid.Validate();

            //Assert
            Assert.True(actual);
        }



        [Fact]
        public void ValidateBadCategory()
        {
            //Arrange
            Transaction invalid = new Transaction(Guid.NewGuid(), -85.55m, Currency.USD, null, "nothing important",null, System.DateTime.Now);

            //Act
            var actual = invalid.Validate();

            //Assert
            Assert.False(actual);
        }

        [Fact]
        public void ValidateBadMoneyAmount()
        {
            //Arrange
            Category validCat = new Category(Guid.NewGuid(), "business", "description here...",Color.Empty,null);
            Transaction invalid = new Transaction(Guid.NewGuid(), 0, Currency.USD, validCat, "nothing important",null, System.DateTime.Now);

            //Act
            var actual = invalid.Validate();

            //Assert
            Assert.False(actual);
        }

        [Fact]
        public void ValidateBadCurrency()
        {
            //Arrange
            Category validCat = new Category(Guid.NewGuid(), "business", "description here...", Color.Empty, null);
            Transaction invalid = new Transaction(Guid.NewGuid(), -85.55m, null, validCat, "nothing important", null, System.DateTime.Now);

            //Act
            var actual = invalid.Validate();

            //Assert
            Assert.False(actual);
        }


     


    }
}
