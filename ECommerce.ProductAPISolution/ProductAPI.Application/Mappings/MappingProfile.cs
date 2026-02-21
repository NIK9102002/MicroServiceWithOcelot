using AutoMapper;
using ProductAPI.Application.DTOs;
using ProductAPI.Domain.Entities;

namespace ProductAPI.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
        }       
    }
}
