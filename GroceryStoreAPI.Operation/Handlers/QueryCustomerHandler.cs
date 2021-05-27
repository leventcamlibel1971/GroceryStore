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
    public class QueryCustomerHandler : IRequestHandler<QueryCustomerRequest, QueryCustomerResponse>
    {
        private readonly IDbContextFactory<GroceryStoreDbContext> _dbContextFactory;

        public QueryCustomerHandler(IDbContextFactory<GroceryStoreDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<QueryCustomerResponse> Handle(QueryCustomerRequest request,
            CancellationToken cancellationToken)
        {
            await using var dbContext = _dbContextFactory.CreateDbContext();

            var customers = await dbContext.Customers
                .Select(CustomerProjection.ToViewModel)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            return new QueryCustomerResponse {Customers = customers};
        }
    }
}