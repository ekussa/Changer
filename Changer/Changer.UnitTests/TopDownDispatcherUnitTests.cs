using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Changer.UnitTests
{
    [TestFixture]
    public class TopDownDispatcherUnitTests
    {
        private MockRepository _repositoryMock;
        private Mock<IStorage> _reserveMock;
        
        private List<decimal> _twoBills;
        private List<decimal> _singleBill;
        private const decimal Hundred = 100m;
        private const decimal Fifty = 50m;
        private const decimal HundredAndFifty = 150;


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
            _reserveMock.
                Setup(_ => _.Distinct).
                Returns(_singleBill);
            
            _reserveMock.
                Setup(_ => _.Contain(Hundred)).
                Returns(true);

            var cashier =
                new TopDownDispatcher(
                    _reserveMock.Object);

            //Act
            var result = cashier.
                ForAmount(Hundred);

            //Assert
            result.Count.Should().Be(1);
            result.Total.Should().Be(Hundred);
            _repositoryMock.VerifyAll();
        }
        
        [TestCase(-99)]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(49)]
        public void ShouldGetNoBill_WhenAmountLowerThanBill(decimal amount)
        {
            //Arrange
            _reserveMock.
                Setup(_ => _.Distinct).
                Returns(_twoBills);
            
            var cashier =
                new TopDownDispatcher(
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
        public void ShouldGetHundredBillOnly_WhenAmountMultiple()
        {
            //Arrange
            const int threeHundred = 300;
            
            _reserveMock.
                Setup(_ => _.Distinct).
                Returns(_twoBills);
            
            _reserveMock.
                Setup(_ => _.Contain(Hundred)).
                Returns(true);

            var cashier =
                new TopDownDispatcher(
                    _reserveMock.Object);

            //Act
            var result = cashier.
                ForAmount(threeHundred);

            //Assert
            result.Count.Should().Be(1);
            result.First().Key.Should().Be(Hundred);
            result.First().Value.Should().Be(3);
            result.Total.Should().Be(threeHundred);
            _repositoryMock.VerifyAll();
        }
        
        [Test]
        public void ShouldGetFiftyBills_WhenHundredRunsOut()
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
                Returns(false);

            var cashier =
                new TopDownDispatcher(
                    _reserveMock.Object);

            //Act
            var result = cashier.
                ForAmount(HundredAndFifty);

            //Assert
            result.Count.Should().Be(1);
            result.Keys.First().Should().Be(Fifty);
            result.Values.First().Should().Be(3);
            result.Total.Should().Be(HundredAndFifty);
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
