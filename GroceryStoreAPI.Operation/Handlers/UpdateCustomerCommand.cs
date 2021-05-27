using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GroceryStoreAPI.Entity;
using GroceryStoreAPI.Operation.Projections;
using GroceryStoreAPI.Operation.Utility;
using GroceryStoreAPI.ViewModel;
using GroceryStoreAPI.ViewModel.Requests;
using GroceryStoreAPI.ViewModel.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GroceryStoreAPI.Operation.Handlers
{
    public class UpdateCustomerCommand : IRequestHandler<UpdateCustomerRequest, UpdateCustomerResponse>
    {
        private readonly IDbContextFactory<GroceryStoreDbContext> _dbContextFactory;

        public UpdateCustomerCommand(IDbContextFactory<GroceryStoreDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<UpdateCustomerResponse> Handle(UpdateCustomerRequest request,
            CancellationToken cancellationToken)
        {
            await using var dbContext = _dbContextFactory.CreateDbContext();

            var customer = await dbContext.Customers
                .Where(x => x.Id == request.Id)
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);

            if (customer == null) throw new BadRequestException("Record is not found");

            CustomerProjection.UpdateEntity(customer, request);

            await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return new UpdateCustomerResponse
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