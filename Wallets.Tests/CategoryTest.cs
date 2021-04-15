using System;
using System.Drawing;
using Wallets.BusinessLayer;
using Xunit;

namespace Wallets.Tests
{
    public class CategoryTest
    {

      
        [Fact]
        public void ValidateValid()
        {
            //Arrange
            Category valid = new Category(Guid.NewGuid(), "business", "description here...", new ColorWrapper(System.Drawing.Color.White), null, Guid.NewGuid());

            //Act
            var actual = valid.Validate();

            //Assert
            Assert.True(actual);
        }


        [Fact]
        public void ValidateBadName()
        {
            //Arrange
            Category invalidName = new Category(Guid.NewGuid(), " ", "description here...", new ColorWrapper(System.Drawing.Color.White), null, Guid.NewGuid());

            //Act
            var actual = invalidName.Validate();

            //Assert
            Assert.False(actual);
        }





    }
}
