using FluentAssertions;
using NUnit.Framework;

namespace Changer.UnitTests
{
    [TestFixture]
    public class BillsUnitTests
    {
        [Test]
        public void ShouldSumBills()
        {
            //Arrange
            const decimal addValue = 100m;
            var bills = new Bills();
            
            //Act
            bills += addValue;
            bills += addValue;

            //Assert
            bills.Count.Should().Be(1);
            bills.Total.Should().Be(addValue*2);
            bills[addValue].Should().Be(2);
        }
        
        [Test]
        public void ShouldSubBills()
        {
            //Arrange
            const decimal startValue = 100m;
            const decimal expectedValue = startValue - startValue;
            
            var bills = new Bills(new []{startValue});
            
            //Act
            bills -= startValue;

            //Assert
            bills.Count.Should().Be(1);
            bills.Total.Should().Be(expectedValue);
            bills[startValue].Should().Be(0);
        }
    }
}
