using Fiap.TechChallenge.Api.Delete.Domain.Contato.Request;
using Fiap.TechChallenge.Api.Delete.Domain.Contato.Result;
using Fiap.TechChallenge.Api.Delete.Domain.DataBaseContext;
using Fiap.TechChallenge.Api.Delete.Infrastructure.MessageBroker;
using Fiap.TechChallenge.Foundation.Core.Exceptions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Fiap.TechChallenge.Api.Delete.Domain.Contato;

public class ContatoService : IContatoService
{
    
    private readonly ILogger<ContatoService> _logger;
    private readonly IMessageBrokerService _messageBrokerService;

    public ContatoService(ILogger<ContatoService> logger, IMessageBrokerService messageBrokerService)
    {
        _logger = logger;
        _messageBrokerService = messageBrokerService;
    }


    public async Task<RemoverContatoResult> RemoverContatoAsync(RemoverContatoRequest request)
    {
        _logger.LogInformation("Iniciando remover de contato");
        try
        {
            // Validação de entrada
            ArgumentNullException.ThrowIfNull(request);
            
            var msg = new RemoverContatoRequest(request.Id);

            // Adicionar na fila
            await _messageBrokerService.ProducerAsync("fiap-remover", JsonConvert.SerializeObject(msg));

            return new RemoverContatoResult { Sucesso = true};
        }
        catch (BusinessException ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar contato.");
            throw new Exception(ex.Message);
        }
    }
}