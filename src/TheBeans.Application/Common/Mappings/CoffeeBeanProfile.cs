using AutoMapper;
using TheBeans.Application.Features.CoffeeBeans.Commands.CreateCoffeeBean;
using TheBeans.Application.Features.CoffeeBeans.Commands.DeleteCoffeeBean;
using TheBeans.Application.Features.CoffeeBeans.Commands.UpdateCoffeeBean;
using TheBeans.Application.Features.CoffeeBeans.Queries.CoffeeBeansSearch;
using TheBeans.Application.Features.CoffeeBeans.Queries.GetCoffeeBeans;

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
            CreateMap<CreateCoffeeBeanCommand, CoffeeBean>()
                .ForMember(dest => dest.Origin, opt => opt.MapFrom(src => src.Country))
                .ForMember(dest => dest.RoastLevel, opt => opt.MapFrom(src => src.Colour))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Cost))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image));


            // Map CoffeeBean to CreateCoffeeBeanCommandResponse
            CreateMap<CoffeeBean, CreateCoffeeBeanCommandResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<CoffeeBean, UpdateCoffeeBeanCommandResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<CoffeeBean, DeleteCoffeeBeanCommandResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<UpdateCoffeeBeanCommand, CoffeeBean>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Ignore Id since it's coming from the command
                .ForMember(dest => dest.Origin, opt => opt.MapFrom(src => src.Country))
                .ForMember(dest => dest.RoastLevel, opt => opt.MapFrom(src => src.Colour))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Cost))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Image));

            CreateMap<CoffeeBean, CoffeeBeanDto>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
               .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => $"{src.Currency}{src.Price}"))
               .ForMember(dest => dest.Colour, opt => opt.MapFrom(src => src.RoastLevel))
               .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.ImageUrl))
               .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Origin));


            CreateMap<CoffeeBean, SearchCoffeeBeanDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => $"{src.Currency}{src.Price}"))
                .ForMember(dest => dest.Colour, opt => opt.MapFrom(src => src.RoastLevel))
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Origin));

            

        }
    }
}
