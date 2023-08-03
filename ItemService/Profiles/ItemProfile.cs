using AutoMapper;
using ItemService.Dtos;
using ItemService.Models;

namespace ItemService.Profiles
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            //Criando Mapeamento de conversao da mensagem de restaurante Service de RestauranteReadDto para Restaurante
            //Pegaremos o IdExterno do restaurante vindo de restaurante service atraves do id do restaurante local
            CreateMap<RestauranteReadDto, Restaurante>()
                .ForMember(destination => destination.IdExterno, options => options.MapFrom(src => src.Id));
            CreateMap<Restaurante, RestauranteReadDto>();
            CreateMap<ItemCreateDto, Item>();
            CreateMap<Item, ItemCreateDto>();
        }
    }
}