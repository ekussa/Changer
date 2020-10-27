using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace Changer.UnitTests
{
    [TestFixture]
    public class LimitedStorageUnitTests
    {
        private readonly Dictionary<decimal, int> _tenToHundred;
        private readonly Dictionary<decimal, int> _ten;

        public LimitedStorageUnitTests()
        {
            _ten = new Dictionary<decimal, int>
            {
                {10m, 1},
            };
            
            _tenToHundred = new Dictionary<decimal, int>
            {
                {10m, 1},
                {50m, 1},
                {100m, 1},
            };
        }
        
        [Test]
        public void ShouldContainBill()
        {
            //Arrange
            var limited = new LimitedStorage(_ten);
            
            //Act
            var result = limited.Contain(
                _ten.Keys.First());

            //Assert
            result.Should().BeTrue();
        }
        
        [Test]
        public void ShouldNotContainBill()
        {
            //Arrange
            var limited = new LimitedStorage(_ten);
            
            //Act
            var result = limited.Contain(11);

            //Assert
            result.Should().BeFalse();
        }
        
        [Test]
        public void ShouldRunOutOfAllBills()
        {
            //Arrange
            var limited = new LimitedStorage(_tenToHundred);
            
            //Act
            var shouldBeTrue1 = limited.Contain(100);
            var shouldBeTrue2 = limited.Contain(50);
            var shouldBeTrue3 = limited.Contain(10);
            var shouldBeFalse1 = limited.Contain(100);
            var shouldBeFalse2 = limited.Contain(50);
            var shouldBeFalse3 = limited.Contain(10);

            //Assert
            shouldBeTrue1.Should().BeTrue();
            shouldBeTrue2.Should().BeTrue();
            shouldBeTrue3.Should().BeTrue();
            shouldBeFalse1.Should().BeFalse();
            shouldBeFalse2.Should().BeFalse();
            shouldBeFalse3.Should().BeFalse();
        }

        [Test]
        public void ShouldGetDistinctBills()
        {
            //Arrange
            var limited = new LimitedStorage(
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
