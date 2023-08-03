//Interface responsavel por processar a mensagem recebida de restaurante service
namespace ItemService.EventProcessor
{
    public interface IProcessaEvento
    {
        void Processa(string mensagem);
    }
}
