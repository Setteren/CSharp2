using System;
using System.Drawing;
using Xunit;
using BusinessLayer;

namespace Tests
{
    public class TransactionTest
    {
        [Fact]
        public void ValidateValid()
        {
            //Arrange
            Category validCat = new Category(1, "business", "description here...",Color.Empty,null);

            Transaction valid = new Transaction(1, -85.55m, Currency.Usd, validCat,"nothing important",null, System.DateTime.Now);

            //Act
            var actual = valid.Validate();

            //Assert
            Assert.True(actual);
        }

        [Fact]
        public void ValidateBadWalletId()
        {
            //Arrange
            Category validCat = new Category(1, "business", "description here...", Color.Empty, null) ;

            Transaction invalid = new Transaction(-1, -85.55m, Currency.Usd, validCat, "nothing important",null, System.DateTime.Now);

            //Act
            var actual = invalid.Validate();

            //Assert
            Assert.False(actual);
        }

        [Fact]
        public void ValidateBadCategory()
        {
            //Arrange
            Transaction invalid = new Transaction(1, -85.55m, Currency.Usd, null, "nothing important",null, System.DateTime.Now);

            //Act
            var actual = invalid.Validate();

            //Assert
            Assert.False(actual);
        }

        [Fact]
        public void ValidateBadMoneyAmount()
        {
            //Arrange
            Category validCat = new Category(1, "business", "description here...",Color.Empty,null);
            Transaction invalid = new Transaction(1, 0, Currency.Usd, validCat, "nothing important",null, System.DateTime.Now);

            //Act
            var actual = invalid.Validate();

            //Assert
            Assert.False(actual);
        }

        [Fact]
        public void ValidateBadCurrency()
        {
            //Arrange
            Category validCat = new Category(1, "business", "description here...", Color.Empty, null);
            Transaction invalid = new Transaction(1, -85.55m, null, validCat, "nothing important", null, System.DateTime.Now);

            //Act
            var actual = invalid.Validate();

            //Assert
            Assert.False(actual);
        }


        /*
           public override bool Validate()
           {
               var result = true;

               if (WalletGuid <= 0)
                   result = false;
               if (String.IsNullOrWhiteSpace(Currency))
                   result = false;
               if (Category == null)
                   result = false;
               if (MoneyAmount == null)
                   result = false;

               return result;
           }

       }
        */

    }
}
