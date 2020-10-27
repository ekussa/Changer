using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Changer.UnitTests
{
    [TestFixture]
    public class BottomUpDispatcherUnitTests
    {
        private const decimal HundredAndFifty = 150m;
        private const decimal Hundred = 100m;
        private const decimal Fifty = 50m;

        private MockRepository _repositoryMock;
        private Mock<IStorage> _reserveMock;
        
        private List<decimal> _twoBills;
        private List<decimal> _singleBill;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new MockRepository(MockBehavior.Strict);
            _reserveMock = _repositoryMock.Create<IStorage>();
            _singleBill = new List<decimal> {Hundred};
            _twoBills = new List<decimal> {Hundred, Fifty};
        }
        
        [Test]
        public void ShouldGetSingleBill_WhenAmountOfSameValue()
        {
            //Arrange
            var hundredBill = _singleBill.First();
            
            _reserveMock.
                Setup(_ => _.Distinct).
                Returns(_singleBill);
            
            _reserveMock.
                Setup(_ => _.Contain(hundredBill)).
                Returns(true);

            var cashier =
                new BottomUpDispatcher(
                    _reserveMock.Object);

            //Act
            var result = cashier.
                ForAmount(hundredBill);

            //Assert
            result.Count.Should().Be(1);
            result.Total.Should().Be(Hundred);
            _repositoryMock.VerifyAll();
        }
        
        [TestCase(-99)]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(99)]
        public void ShouldGetNoBill_WhenAmountLowerThanBill(decimal amount)
        {
            //Arrange
            _reserveMock.
                Setup(_ => _.Distinct).
                Returns(_singleBill);
            
            var cashier =
                new BottomUpDispatcher(
                    _reserveMock.Object);

            //Act
            var result = cashier.
                ForAmount(amount);

            //Assert
            result.Count.Should().Be(0);
            result.Total.Should().Be(0);
            _repositoryMock.VerifyAll();
        }

        [Test]
        public void ShouldGetFiftyBillOnly_WhenAmountMultiple()
        {
            //Arrange
            _reserveMock.
                Setup(_ => _.Distinct).
                Returns(_twoBills);
            
            _reserveMock.
                Setup(_ => _.Contain(_twoBills.Min())).
                Returns(true);

            var cashier =
                new BottomUpDispatcher(
                    _reserveMock.Object);

            var sum = _twoBills.Sum();
            
            //Act
            var result = cashier.
                ForAmount(sum);

            //Assert
            result.Count.Should().Be(1);
            result.First().Value.Should().Be(3);
            result.Total.Should().Be(sum);
            _repositoryMock.VerifyAll();
        }
        
        [Test]
        public void ShouldGetHundredBills_WhenFiftyRunsOut()
        {
            //Arrange
            _reserveMock.
                Setup(_ => _.Distinct).
                Returns(_twoBills);
            
            _reserveMock.
                Setup(_ => _.Contain(Hundred)).
                Returns(true);

            _reserveMock.
                Setup(_ => _.Contain(Fifty)).
                Returns(false);

            var cashier =
                new BottomUpDispatcher(
                    _reserveMock.Object);

            var sum = _twoBills.Sum();
            
            //Act
            var result = cashier.
                ForAmount(sum);

            //Assert
            result.Count.Should().Be(1);
            result.Total.Should().Be(Hundred);
            _repositoryMock.VerifyAll();
        }
        
        [Test]
        public void ShouldGetHundredAndFiftyBills_WHenAmountAndBillsMatches()
        {
            //Arrange
            _reserveMock.
                Setup(_ => _.Distinct).
                Returns(_twoBills);
            
            _reserveMock.
                Setup(_ => _.Contain(Fifty)).
                Returns(true);

            _reserveMock.
                Setup(_ => _.Contain(Hundred)).
                Returns(true);

            var cashier =
                new TopDownDispatcher(
                    _reserveMock.Object);

            //Act
            var result = cashier.
                ForAmount(HundredAndFifty);

            //Assert
            result.Count.Should().Be(2);
            result[Hundred].Should().Be(1);
            result[Fifty].Should().Be(1);
            result.Total.Should().Be(HundredAndFifty);
            _repositoryMock.VerifyAll();
        }
    }
}
