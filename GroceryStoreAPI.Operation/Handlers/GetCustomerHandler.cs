using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GroceryStoreAPI.Entity;
using GroceryStoreAPI.Operation.Projections;
using GroceryStoreAPI.ViewModel.Requests;
using GroceryStoreAPI.ViewModel.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GroceryStoreAPI.Operation.Handlers
{
    public class GetCustomerHandler : IRequestHandler<GetCustomerRequest, GetCustomerResponse>
    {
        private readonly IDbContextFactory<GroceryStoreDbContext> _dbContextFactory;

        public GetCustomerHandler(IDbContextFactory<GroceryStoreDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<GetCustomerResponse> Handle(GetCustomerRequest request, CancellationToken cancellationToken)
        {
            await using var dbContext = _dbContextFactory.CreateDbContext();

            var customer = await dbContext.Customers
                .Where(x => x.Id == request.Id)
                .Select(CustomerProjection.ToViewModel)
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);

            return new GetCustomerResponse
            {
                Customer = customer
            };
        }
    }
}