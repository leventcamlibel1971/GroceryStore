using FluentAssertions;
using GroceryStoreAPI.ViewModel.Requests;
using Xunit;

namespace GroceryStoreAPI.Tests.UnitTests
{
    public class InputValidationUnitTests
    {
        [Fact]
        public void GetCustomerRequestValidator_Should_Be_Valid()
        {
            GetCustomerRequestValidator validator = new();

            var request = new GetCustomerRequest
            {
                Id = 1
            };
            var validated = validator.Validate(request);
            validated.IsValid.Should().BeTrue();
        }

        [Fact]
        public void GetCustomerRequestValidator_Should_Be_InValid()
        {
            GetCustomerRequestValidator validator = new();

            var request = new GetCustomerRequest
            {
                Id = 0
            };
            var validated = validator.Validate(request);
            validated.IsValid.Should().BeFalse();
        }

        [Fact]
        public void UpdateCustomerRequestValidator_Should_Be_Valid()
        {
            UpdateCustomerRequestValidator validator = new();

            var request = new UpdateCustomerRequest
            {
                Id = 1,
                Name = "test"
            };
            var validated = validator.Validate(request);
            validated.IsValid.Should().BeTrue();
        }

        [Fact]
        public void UpdateCustomerRequestValidator_Should_Be_InValid()
        {
            UpdateCustomerRequestValidator validator = new();

            var request = new UpdateCustomerRequest
            {
                Id = 5,
                Name = null
            };
            var validated = validator.Validate(request);
            validated.IsValid.Should().BeFalse();
        }

        [Fact]
        public void CreateCustomerRequestValidator_Should_Be_Valid()
        {
            CreateCustomerRequestValidator validator = new();

            var request = new CreateCustomerRequest
            {
                Name = "test"
            };
            var validated = validator.Validate(request);
            validated.IsValid.Should().BeTrue();
        }

        [Fact]
        public void CreateCustomerRequestValidator_Should_Be_InValid()
        {
            CreateCustomerRequestValidator validator = new();

            var request = new CreateCustomerRequest
            {
                Name = null
            };
            var validated = validator.Validate(request);
            validated.IsValid.Should().BeFalse();
        }
    }
}