using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TheBeans.Application.Features.CoffeeBeans.Commands.CreateCoffeeBean;

namespace TheBeans.Application.Common.Mappings
{
    /// <summary>
    /// AutoMapper profile for mapping between CoffeeBean-related models.
    /// </summary>
    public class CoffeeBeanProfile : Profile
    {
        /// <summary>
        /// Configures the mapping between CreateCoffeeBeanCommand, CoffeeBean, 
        /// and CreateCoffeeBeanCommandResponse.
        /// </summary>
        public CoffeeBeanProfile()
        {
            // Map CreateCoffeeBeanCommand to CoffeeBean
            CreateMap<CreateCoffeeBeanCommand, CoffeeBean>();

            // Map CoffeeBean to CreateCoffeeBeanCommandResponse
            CreateMap<CoffeeBean, CreateCoffeeBeanCommandResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        }
    }
}