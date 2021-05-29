using System;
using System.Threading.Tasks;
using FluentAssertions;
using GroceryStoreAPI.Operation.Handlers;
using GroceryStoreAPI.Operation.Utility;
using GroceryStoreAPI.ViewModel.Requests;
using Xunit;

namespace GroceryStoreAPI.Tests.UnitTests
{
    [Collection("Sequential")]
    public class OperationUnitTests : DatabaseFixture
    {
        [Fact]
        public async Task GetCustomerHandle_Should_Return_Data()
        {
            var getCustomerHandler = new GetCustomerHandler(dbContextFactory);
            var customerResponse = await getCustomerHandler.Handle(new GetCustomerRequest
            {
                Id = 1
            }, default);

            customerResponse.Customer.Should().NotBeNull();
            customerResponse.Customer.Id.Should().Be(1);
        }

        [Fact]
        public async Task GetCustomerHandle_Should_Throw_Exception()
        {
            var getCustomerHandler = new GetCustomerHandler(dbContextFactory);
            Exception ex = await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                getCustomerHandler.Handle(new GetCustomerRequest
                {
                    Id = 0
                }, default));

            ex.Message.Should().Be("customer is not found.");
        }

        [Fact]
        public async Task QueryCustomerHandle_Should_Return_Data()
        {
            var queryCustomerHandler = new QueryCustomerHandler(dbContextFactory);
            var queryCustomerResponse = await queryCustomerHandler.Handle(new QueryCustomerRequest(), default);

            queryCustomerResponse.Customers.Should().NotBeNull();
            queryCustomerResponse.Customers.Count.Should().Be(6);
        }

        [Fact]
        public async Task UpdateCustomerCommand_Should_Update_Name()
        {
            var updateCustomerCommand = new UpdateCustomerCommand(dbContextFactory);

            var customerName = Guid.NewGuid().ToString();
            var updateCustomerResponse = await updateCustomerCommand.Handle(new UpdateCustomerRequest
            {
                Id = 1,
                Name = customerName
            }, default);

            var getCustomerHandler = new GetCustomerHandler(dbContextFactory);

            var customerAfterUpdatedResponse = await getCustomerHandler.Handle(new GetCustomerRequest
            {
                Id = 1
            }, default);

            updateCustomerResponse.Customer.Id.Should().Be(customerAfterUpdatedResponse.Customer.Id);
            updateCustomerResponse.Customer.Name.Should().Be(customerName);
        }

        [Fact]
        public async Task UpdateCustomerCommand_Should_Throw_Exception()
        {
            var updateCustomerCommand = new UpdateCustomerCommand(dbContextFactory);

            var customerName = Guid.NewGuid().ToString();

            Exception ex = await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                updateCustomerCommand.Handle(new UpdateCustomerRequest
                {
                    Id = 0,
                    Name = customerName
                }, default));

            ex.Message.Should().Be("Record is not found");
        }

        [Fact]
        public async Task CreateCustomerCommand_Should_Create_Record()
        {
            var createCustomerCommand = new CreateCustomerCommand(dbContextFactory);

            var customerName = Guid.NewGuid().ToString();
            var createCustomerResponse = await createCustomerCommand.Handle(new CreateCustomerRequest
            {
                Name = customerName
            }, default);

            var getCustomerHandler = new GetCustomerHandler(dbContextFactory);

            var customerAfterUpdatedResponse = await getCustomerHandler.Handle(new GetCustomerRequest
            {
                Id = createCustomerResponse.Customer.Id
            }, default);

            createCustomerResponse.Customer.Id.Should().Be(customerAfterUpdatedResponse.Customer.Id);
            createCustomerResponse.Customer.Name.Should().Be(customerName);
        }
    }
}