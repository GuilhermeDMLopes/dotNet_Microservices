//Classe responsavel por processar a mensagem recebida de restaurante service
using AutoMapper;
using ItemService.Data;
using ItemService.Dtos;
using ItemService.Models;
using System.Text.Json;

namespace ItemService.EventProcessor
{
    public class ProcessaEvento : IProcessaEvento
    {
        private readonly IMapper _mapper;
        //Como o banco de ItemService utiliza Scopped, nosso serviço roda em background, teremos um problema na hora de comunicar com o banco
        //Para resolver o problema, fazemos
        private readonly IServiceScopeFactory _scopeFactory;

        public ProcessaEvento(IMapper mapper, IServiceScopeFactory scopeFactory)
        {
            _mapper = mapper;
            _scopeFactory = scopeFactory;
        }

        public void Processa(string mensagem)
        {
            //Salvando o restaurante recebido por RestauranteService no banco
            //Criando um contexto
            using var scope = _scopeFactory.CreateScope();

            //Nosso repositorio de item recebe o contexto
            var itemRepository = scope.ServiceProvider.GetRequiredService<IItemRepository>();

            //Convertendo a mensagem em string para restauranteReadDto e inserir no banco
            var restauranteReadDto = JsonSerializer.Deserialize<RestauranteReadDto>(mensagem);

            //Mapear esse dto para o objeto modelo de Restaurante
            var restaurante = _mapper.Map<Restaurante>(restauranteReadDto);

            //Fazendo o cadastro no banco
            //Se não houver o restaurante de id tal no banco, salvamos ele
            if(itemRepository.ExisteRestauranteExterno(restaurante.Id))
            {
                itemRepository.CreateRestaurante(restaurante);
                itemRepository.SaveChanges();
            }
           
        }
    }
}
