using System.Threading.Tasks;
using GroceryStoreAPI.Filters;
using GroceryStoreAPI.ViewModel.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GroceryStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ServiceFilter(typeof(ModelStateValidate))]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var customerResponse = await _mediator.Send(new GetCustomerRequest
            {
                Id = id
            });

            return Ok(customerResponse);
        }

        [HttpGet("all")]
        public async Task<IActionResult> AllCustomers()
        {
            var customersResponse = await _mediator.Send(new QueryCustomerRequest());

            return Ok(customersResponse);
        }

        [ServiceFilter(typeof(ModelStateValidate))]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateCustomerRequest request)
        {
            var createResponse = await _mediator.Send(request);

            return CreatedAtAction(nameof(Get), new {id = createResponse.Customer.Id}, createResponse);
        }

        [ServiceFilter(typeof(ModelStateValidate))]
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateCustomerRequest request)
        {
            var updateResponse = await _mediator.Send(request);

            return Ok(updateResponse);
        }
    }
}