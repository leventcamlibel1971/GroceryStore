using System.Collections.Generic;

namespace GroceryStoreAPI.ViewModel.Responses
{
    public class QueryCustomerResponse
    {
        public List<CustomerViewModel> Customers { get; set; }
    }
}