using MediatR;
using Microsoft.AspNetCore.Mvc;
using TheBeans.Application.Features.CoffeeBeans.Commands.CreateCoffeeBean;
using TheBeans.Application.Features.CoffeeBeans.Commands.DeleteCoffeeBean;
using TheBeans.Application.Features.CoffeeBeans.Commands.UpdateCoffeeBean;
using TheBeans.Application.Features.CoffeeBeans.Queries.GetCoffeeBeans;

namespace TheBeans.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoffeeBeansController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CoffeeBeansController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("CoffeeBean/Add")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCoffeeBeanCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [Route("CoffeeBean/Update")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateCoffeeBeanCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [Route("CoffeeBean/Delete")]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteCoffeeBeanCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [Route("CoffeeBean/GetAll")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetCoffeeBeansQuery query)
        {
            var response = await _mediator.Send(query);
            return Ok(response);
        }

    }
}