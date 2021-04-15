using Xunit;
using System.Drawing;
using Wallets.BusinessLayer;

namespace Tests
{
    public class CategoryTest
    {
        [Fact]
        public void ValidateValid()
        {
            //Arrange
            Category valid = new Category(1, "business", "description here...", System.Drawing.Color.White, null);

            //Act
            var actual = valid.Validate();

            //Assert
            Assert.True(actual);
        }

        [Fact]
        public void ValidateBadId()
        {
            //Arrange
            Category invalidId = new Category(-1, "business", "description here...", System.Drawing.Color.White, null);

            //Act
            var actual = invalidId.Validate();

            //Assert
            Assert.False(actual);
        }


        [Fact]
        public void ValidateBadName()
        {
            //Arrange
            Category invalidName = new Category(2, "  ", "description here...", System.Drawing.Color.White, null);

            //Act
            var actual = invalidName.Validate();

            //Assert
            Assert.False(actual);
        }

    }
}
