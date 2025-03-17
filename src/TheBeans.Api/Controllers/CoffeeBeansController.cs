using MediatR;
using Microsoft.AspNetCore.Mvc;
using TheBeans.Application.Features.CoffeeBeans.Commands.CreateCoffeeBean;

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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCoffeeBeanCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    }
}