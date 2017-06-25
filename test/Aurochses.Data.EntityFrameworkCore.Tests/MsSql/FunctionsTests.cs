using Aurochses.Data.EntityFrameworkCore.MsSql;
using Xunit;

namespace Aurochses.Data.EntityFrameworkCore.Tests.MsSql
{
    public class FunctionsTests
    {
        [Fact]
        public void NewSequentialId_Value_Equals()
        {
            // Arrange & Act & Assert
            Assert.Equal("newsequentialid()", Functions.NewSequentialId);
        }

        [Fact]
        public void GetUtcDate_Value_Equals()
        {
            // Arrange & Act & Assert
            Assert.Equal("getutcdate()", Functions.GetUtcDate);
        }
    }
}