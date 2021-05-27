using GroceryStoreAPI.ViewModel.Responses;
using MediatR;

namespace GroceryStoreAPI.ViewModel.Requests
{
    public class QueryCustomerRequest : IRequest<QueryCustomerResponse>
    {
    }
}