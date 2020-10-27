using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Changer.UnitTests
{
    [TestFixture]
    public class PrioritizedDispatchersUnitTests
    {
        private const decimal Hundred = 100m;
        private const decimal Fifty = 50m;
        private const decimal Twenty = 20m;
        private const decimal Ten = 10m;
        private const decimal Five = 5m;
        private const decimal One = 1m;

        private MockRepository _repositoryMock;
        private Mock<IStorage> _reserveMock;
        
        private List<decimal> _bills;
        
        [SetUp]
        public void Setup()
        {
            _repositoryMock = new MockRepository(MockBehavior.Strict);
            _reserveMock = _repositoryMock.Create<IStorage>();
            _bills = new List<decimal>
            {
                Hundred, Fifty, Twenty, Ten, Five, One
            };
        }


        private void MockBills()
        {
            _reserveMock.
                Setup(_ => _.Distinct).
                Returns(_bills);
            
            _reserveMock.
                Setup(_ => _.Contain(Hundred)).
                Returns(true);

            _reserveMock.
                Setup(_ => _.Contain(Fifty)).
                Returns(true);

            _reserveMock.
                Setup(_ => _.Contain(Twenty)).
                Returns(true);
            
            _reserveMock.
                Setup(_ => _.Contain(Ten)).
                Returns(true);
            
            _reserveMock.
                Setup(_ => _.Contain(Five)).
                Returns(true);
            
            _reserveMock.
                Setup(_ => _.Contain(One)).
                Returns(true);
        }
        
        [Test]
        public void ShouldGetPrioritizedTopDownBillsFirst()
        {
            //Arrange
            var priority = new [] {
                Fifty
            };
            
            var cashier =
                new PrioritizedTopDownDispatcher(
                    _reserveMock.Object,
                    priority
                    );

            MockBills();

            var total = _bills.Sum() * 2;
            
            //Act
            var result = cashier.
                ForAmount(total);

            //Assert
            result.Count.Should().Be(3);
            result[Fifty].Should().Be(7);
            result[Twenty].Should().Be(1);
            result[One].Should().Be(2);
            result.Total.Should().Be(total);
        }
        
        [Test]
        public void ShouldGetPrioritizedBottomUpBillsFirst()
        {
            //Arrange
            var priority = new [] {
                Fifty
            };
            
            var cashier =
                new PrioritizedBottomUpDispatcher(
                    _reserveMock.Object,
                    priority
                );

            MockBills();

            var total = _bills.Sum() * 2;
            
            //Act
            var result = cashier.
                ForAmount(total);

            //Assert
            result.Count.Should().Be(2);
            result[Fifty].Should().Be(7);
            result[One].Should().Be(22);
            result.Total.Should().Be(total);
        }
    }
}
