using System;
using System.Linq.Expressions;
using GroceryStoreAPI.Entity.Domain;
using GroceryStoreAPI.ViewModel;
using GroceryStoreAPI.ViewModel.Requests;

namespace GroceryStoreAPI.Operation.Projections
{
    public class CustomerProjection
    {
        public static Func<CreateCustomerRequest, Customer> CreateEntity
            = viewModel => new Customer
            {
                Name = viewModel.Name
            };

        public static Action<Customer, UpdateCustomerRequest> UpdateEntity
            = (entity, viewModel) => { entity.Name = viewModel.Name; };


        public static Expression<Func<Customer, CustomerViewModel>> ToViewModel
        {
            get
            {
                return entity => new CustomerViewModel
                {
                    Id = entity.Id,
                    Name = entity.Name
                };
            }
        }
    }
}