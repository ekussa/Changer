using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Changer.UnitTests
{
    [TestFixture]
    public class LimitlessStorageUnitTests
    {
        private readonly Bills _ten;
        private readonly Bills _tenToHundred;

        public LimitlessStorageUnitTests()
        {
            _ten = new Bills(
                new[]{10m});

            _tenToHundred = new Bills(
                new[]
                {
                    10m,
                    20m,
                    50m,
                    100m,
                });
        }
        
        [Test]
        public void ShouldContainBill()
        {
            //Arrange
            var limited = new LimitlessStorage(_ten);
            
            //Act
            var result = limited.Contain(
                _ten.First().Key);

            //Assert
            result.Should().BeTrue();
        }
        
        [Test]
        public void ShouldNotContainBill()
        {
            //Arrange
            var limited = new LimitlessStorage(_ten);
            
            //Act
            var result = limited.Contain(11);

            //Assert
            result.Should().BeFalse();
        }
        
        [Test]
        public void ShouldRunOutOfBills()
        {
            //Arrange
            var limited = new LimitlessStorage(_ten);
            
            //Act
            var shouldBeTrue = limited.Contain(
                _ten.First().Key);
            shouldBeTrue |= limited.Contain(
                _ten.First().Key);

            //Assert
            shouldBeTrue.Should().BeTrue();
        }
        
        [Test]
        public void ShouldGetDistinctBills()
        {
            //Arrange
            var limited = new LimitlessStorage(
                _tenToHundred);
            
            //Act
            var result = limited.Distinct;

            //Assert
            result.All(_ =>
                    _tenToHundred.ContainsKey(_)).
                Should().BeTrue();
        }
    }
}
