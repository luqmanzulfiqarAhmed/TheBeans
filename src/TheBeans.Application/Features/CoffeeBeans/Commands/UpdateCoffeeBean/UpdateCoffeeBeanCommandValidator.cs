using FluentValidation;

namespace TheBeans.Application.Features.CoffeeBeans.Commands.UpdateCoffeeBean
{
 
    public class UpdateCoffeeBeanCommandValidator : AbstractValidator<UpdateCoffeeBeanCommand>
    {
        /// <summary>
        /// Initializes validation rules for UpdateCoffeeBeanCommand properties.
        /// </summary>
        public UpdateCoffeeBeanCommandValidator()
        {

            // Ensure Id is provided
            RuleFor(x => x.Id).NotEmpty();

            // Ensure Name is not empty and does not exceed 100 characters
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
            
            // Ensure Currency is provided
            RuleFor(x => x.Currency).NotEmpty();
            
            // Ensure Description is provided
            RuleFor(x => x.Description).NotEmpty();
            
            // Ensure Cost is greater than zero
            RuleFor(x => x.Cost).GreaterThan(0);
            
            // Ensure Colour is provided
            RuleFor(x => x.Colour).NotEmpty();
            
            // Ensure Country is provided
            RuleFor(x => x.Country).NotEmpty();
            
            // Ensure Image is provided
            RuleFor(x => x.Image).NotEmpty();
        }
    }
}

