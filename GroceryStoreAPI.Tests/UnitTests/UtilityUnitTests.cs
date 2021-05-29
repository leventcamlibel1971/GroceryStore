using System;
using FluentAssertions;
using GroceryStoreAPI.Operation.Utility;
using Xunit;

namespace GroceryStoreAPI.Tests.UnitTests
{
    public class UtilityUnitTests
    {
        [Fact]
        public void EntityNotFoundException_Should_Be_ExceptionType()
        {
            var entityNotFoundException = new EntityNotFoundException();

            var isException = entityNotFoundException is Exception;

            isException.Should().BeTrue();
        }

        [Fact]
        public void EntityNotFoundException_Should_Return_Message()
        {
            var message = "testing the exception";
            var entityNotFoundException = new EntityNotFoundException(message);

            entityNotFoundException.Message.Should().Be(message);
        }
    }
}