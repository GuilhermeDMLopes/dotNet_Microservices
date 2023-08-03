//Interface responsavel por enviar requisição http de restaurante para itemService
using RestauranteService.Dtos;

namespace RestauranteService.ItemServiceHttpClient
{
    public interface IItemServiceHttpClient
    {
        public void EnviaRestauranteParaItemService(RestauranteReadDto readDto);
    }
}
