using System;
using System.Threading.Tasks;
using GroceryStoreAPI.Operation.Utility;
using GroceryStoreAPI.ViewModel.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GroceryStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : BaseController
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly IMediator _mediator;

        public CustomerController(ILogger<CustomerController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            try
            {
                var customer = await _mediator.Send(new GetCustomerRequest
                {
                    Id = id
                });

                return Ok(customer);
            }
            catch (Exception ex)
            {
                return GenerateInternalServerError(_logger, ex, nameof(GetCustomer));
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> AllCustomers()
        {
            try
            {
                var customers = await _mediator.Send(new QueryCustomerRequest());

                return Ok(customers);
            }
            catch (Exception ex)
            {
                return GenerateInternalServerError(_logger, ex, nameof(AllCustomers));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest request)
        {
            try
            {
                if (!ModelState.IsValid) return GenerateModelStateError(ModelState, _logger, nameof(CreateCustomer));

                var newCustomer = await _mediator.Send(request);

                return Ok(newCustomer);
            }
            catch (Exception ex)
            {
                return GenerateInternalServerError(_logger, ex, nameof(CreateCustomer));
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCustomer([FromBody] UpdateCustomerRequest request)
        {
            try
            {
                if (!ModelState.IsValid) return GenerateModelStateError(ModelState, _logger, nameof(UpdateCustomer));

                var updatedCustomer = await _mediator.Send(request);

                return Ok(updatedCustomer);
            }
            catch (BadRequestException badRequestException)
            {
                return GenerateBadRequestError(_logger, badRequestException, nameof(UpdateCustomer),
                    "Update customer is failed");
            }
            catch (Exception ex)
            {
                return GenerateInternalServerError(_logger, ex, nameof(UpdateCustomer));
            }
        }
    }
}