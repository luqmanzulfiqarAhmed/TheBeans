using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace TheBeans.Application.Features.CoffeeBeans.Queries.GetCoffeeBeans
{
    public class GetCoffeeBeansQuery : IRequest<List<CoffeeBeanDto>>
    {
    }
}