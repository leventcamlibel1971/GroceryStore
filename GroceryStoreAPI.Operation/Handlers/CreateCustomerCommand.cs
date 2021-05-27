using System.Threading;
using System.Threading.Tasks;
using GroceryStoreAPI.Entity;
using GroceryStoreAPI.Operation.Projections;
using GroceryStoreAPI.ViewModel;
using GroceryStoreAPI.ViewModel.Requests;
using GroceryStoreAPI.ViewModel.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GroceryStoreAPI.Operation.Handlers
{
    public class CreateCustomerCommand : IRequestHandler<CreateCustomerRequest, CreateCustomerResponse>
    {
        private readonly IDbContextFactory<GroceryStoreDbContext> _dbContextFactory;

        public CreateCustomerCommand(IDbContextFactory<GroceryStoreDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<CreateCustomerResponse> Handle(CreateCustomerRequest request,
            CancellationToken cancellationToken)
        {
            await using var dbContext = _dbContextFactory.CreateDbContext();

            var customer = CustomerProjection.CreateEntity(request);

            await dbContext.Customers.AddAsync(customer, cancellationToken).ConfigureAwait(false);

            await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return new CreateCustomerResponse
            {
                Customer = new CustomerViewModel
                {
                    Id = customer.Id,
                    Name = customer.Name
                }
            };
        }
    }
}