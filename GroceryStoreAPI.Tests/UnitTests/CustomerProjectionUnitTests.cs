using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using GroceryStoreAPI.Entity.Domain;
using GroceryStoreAPI.Operation.Projections;
using GroceryStoreAPI.ViewModel;
using GroceryStoreAPI.ViewModel.Requests;
using Xunit;

namespace GroceryStoreAPI.Tests.UnitTests
{
    public class CustomerProjectionUnitTests
    {
        [Fact]
        public void CreateEntity_Should_Create_Entity()
        {
            CreateCustomerRequest createCustomerRequest = new()
            {
                Name = "Test"
            };

            var customer=CustomerProjection.CreateEntity(createCustomerRequest);

            customer.Name.Should().Be(createCustomerRequest.Name);
        }

        [Fact]
        public void UpdateEntity_Should_Update_Entity()
        {
            UpdateCustomerRequest updateCustomerRequest = new()
            {
                Name = "Test"
            };
            Customer customer = new()
            {
                Name = "Joe"
            };
            CustomerProjection.UpdateEntity(customer,updateCustomerRequest);

            customer.Name.Should().Be(updateCustomerRequest.Name);
        }
    }
}
