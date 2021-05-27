using FluentValidation;
using GroceryStoreAPI.ViewModel.Responses;
using MediatR;

namespace GroceryStoreAPI.ViewModel.Requests
{
    public class UpdateCustomerRequest : CustomerViewModel, IRequest<UpdateCustomerResponse>
    {
    }

    public class UpdateCustomerRequestValidator : AbstractValidator<UpdateCustomerRequest>
    {
        public UpdateCustomerRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Id cannot be null or zero.");

            RuleFor(x => x.Name)
                .NotEmpty().NotNull();
        }
    }
}