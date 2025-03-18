using FluentValidation;

namespace TheBeans.Application.Features.CoffeeBeans.Commands.DeleteCoffeeBean
{
 
    public class DeleteCoffeeBeanCommandValidator : AbstractValidator<DeleteCoffeeBeanCommand>
    {
        /// <summary>
        /// Initializes validation rules for DeleteCoffeeBeanCommand properties.
        /// </summary>
        public DeleteCoffeeBeanCommandValidator()
        {

            // Ensure Id is provided
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}

