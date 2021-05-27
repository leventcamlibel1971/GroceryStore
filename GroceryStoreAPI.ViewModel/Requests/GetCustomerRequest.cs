using FluentValidation;
using GroceryStoreAPI.ViewModel.Responses;
using MediatR;

namespace GroceryStoreAPI.ViewModel.Requests
{
    public class GetCustomerRequest : IRequest<GetCustomerResponse>
    {
        public int Id { get; set; }
    }

    public class GetCustomerRequestValidator : AbstractValidator<GetCustomerRequest>
    {
        public GetCustomerRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id cannot be null or zero.");
        }
    }
}