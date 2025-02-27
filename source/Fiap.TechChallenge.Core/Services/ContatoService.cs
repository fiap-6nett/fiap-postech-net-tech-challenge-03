using Fiap.TechChallenge.Core.Contracts.Requests;
using Fiap.TechChallenge.Core.Contracts.Results;
using Fiap.TechChallenge.Core.Entities;
using Fiap.TechChallenge.Core.Messaging;
using Fiap.TechChallenge.Foundation.Core.Exceptions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Fiap.TechChallenge.Core.Services
{
    public class ContatoService : IContatoService
    {
        private readonly ILogger<ContatoService> _logger;
        private readonly IMessageBrokerService _messageBrokerService;

        public ContatoService(ILogger<ContatoService> logger, IMessageBrokerService messageBrokerService)
        {
            _logger = logger;
            _messageBrokerService = messageBrokerService;
        }

        public async Task<CriarContatoResult> CriarContatoAsync(CriarContatoRequest request)
        {
            _logger.LogInformation("Iniciando criação de contato");
            try
            {
                // Validação de entrada
                ArgumentNullException.ThrowIfNull(request);

                // Verificar se o contato já existe
                //var contatoExistente = await _contatoQueryStore.ContatoJaCadastradoAsync(request.Email, request.Telefone, request.DDD);
                //if (contatoExistente) throw new BusinessException("Contato já cadastrado");

                // Criar entidade de contato
                var contato = new ContatoEntity(Guid.NewGuid(), request.Nome, request.Telefone, request.Email, request.DDD);

                // Adicionar na fila
                await _messageBrokerService.ProducerAsync("fiap-cadastro", JsonConvert.SerializeObject(contato));

                return new CriarContatoResult { Contato = contato };
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
}
