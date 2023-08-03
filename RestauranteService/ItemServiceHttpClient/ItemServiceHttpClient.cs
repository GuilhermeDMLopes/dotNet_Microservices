//Classe que ira implementar a interface de envio de requisição http para itemService
using RestauranteService.Dtos;
using System.Text;
using System.Text.Json;

namespace RestauranteService.ItemServiceHttpClient
{
    public class ItemServiceHttpClient : IItemServiceHttpClient
    {
        //Client Http do proprio dotNet
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;

        public ItemServiceHttpClient(HttpClient client, IConfiguration configuration)
        {
            _client = client;
            _configuration = configuration;
        }

        public async void EnviaRestauranteParaItemService(RestauranteReadDto readDto)
        {
            //Tornando o dto enviavel por requisição http através de serialização
            var conteudoHttp = new StringContent
                (
                    JsonSerializer.Serialize(readDto),
                    Encoding.UTF8,
                    //Terminologia de que estamos enviando um arquivo json
                    "application/json"
                );

            //Fazendo o envio da requisição via http
            //Precisamos configurar para onde enviaremos a requisição
            //Criamos uma variavel chamada ItemService em appsettings com o endereço onde enviaremos a requisição (No caso, a rota de RestauranteController em ItemService)
            //Quando enviarmos essa requisição, o metodo post dentro daquele controllador irá cadastrar o novo resturante para ser utilizando pelo ItemService
            await _client.PostAsync(_configuration["ItemService"], conteudoHttp);
        }
    }
}
