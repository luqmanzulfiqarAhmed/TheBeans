using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace TheBeans.Application.Features.CoffeeBeans.Queries.CoffeeBeansSearch
{
    public record SearchCoffeeBeansQuery
    (
        string Name
    )
    : IRequest<List<SearchCoffeeBeanDto>>;

}