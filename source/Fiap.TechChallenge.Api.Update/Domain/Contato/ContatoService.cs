using Fiap.TechChallenge.Api.Update.Domain.Contato.Request;
using Fiap.TechChallenge.Api.Update.Domain.Contato.Result;
using Fiap.TechChallenge.Api.Update.Domain.DataBaseContext;
using Fiap.TechChallenge.Api.Update.Infrastructure.MessageBroker;
using Fiap.TechChallenge.Foundation.Core.Exceptions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Fiap.TechChallenge.Api.Update.Domain.Contato;

public class ContatoService : IContatoService
{
    
    private readonly ILogger<ContatoService> _logger;
    private readonly IMessageBrokerService _messageBrokerService;

    public ContatoService(ILogger<ContatoService> logger, IMessageBrokerService messageBrokerService)
    {
        _logger = logger;
        _messageBrokerService = messageBrokerService;
    }

    public async Task<AtualizarContatoResult> AtualizarContatoAsync(AtualizarContatoRequest request)
    {
        _logger.LogInformation("Iniciando atualizacao de contato");
        try
        {
            // Validação de entrada
            ArgumentNullException.ThrowIfNull(request);

            // Criar entidade de contato
            var contato = new ContatoEntity(Guid.NewGuid(), request.Nome, request.Telefone, request.Email, request.DDD);

            // Adicionar na fila
            await _messageBrokerService.ProducerAsync("fiap-atualizar", JsonConvert.SerializeObject(contato));

            return new AtualizarContatoResult { Contato = contato };
        }
        catch (BusinessException ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualziar contato.");
            throw new Exception(ex.Message);
        }
    }
}