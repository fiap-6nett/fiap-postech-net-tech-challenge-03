﻿using Fiap.TechChallenge.Core.Contracts.Requests;
using Fiap.TechChallenge.Core.Contracts.Results;
using Fiap.TechChallenge.Core.Data.CommandStores;
using Fiap.TechChallenge.Core.Data.QueryStores;
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
        private readonly IContatoQueryStore _contatoQueryStore;
        private readonly IMessageBrokerService _messageBrokerService;
        private readonly IContatoCommandStore _contatoCommandStore;

        public ContatoService(ILogger<ContatoService> logger, IMessageBrokerService messageBrokerService, IContatoQueryStore contatoQueryStore, IContatoCommandStore contatoCommandStore)
        {
            _logger = logger;
            _messageBrokerService = messageBrokerService;
            _contatoQueryStore = contatoQueryStore;
            _contatoCommandStore = contatoCommandStore;
        }

        #region Queue Methods

        public async Task<CriarContatoResult> CriarContatoQueueAsync(CriarContatoRequest request)
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
                _logger.LogError($"Erro ao criar contato. Falha {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public async Task<RemoverContatoResult> RemoverContatoQueueAsync(RemoverContatoRequest request)
        {
            _logger.LogInformation("Iniciando envio de comando para remoção do contato com ID: {Id}", request.Id);
            try
            {
                // Validação de entrada
                ArgumentNullException.ThrowIfNull(request);

                var msg = new RemoverContatoRequest(request.Id);

                // Adicionar na fila
                await _messageBrokerService.ProducerAsync("fiap-remover", JsonConvert.SerializeObject(msg));

                return new RemoverContatoResult { Sucesso = true };
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao remover contato. Falha: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        public async Task<ObterContatoPorIdResult> ObterContatoPorIdQueueAsync(ObterContatoPorIdRequest request)
        {
            _logger.LogInformation("Iniciando obtenção do contato com ID: {Id}", request.Id);
            try
            {
                // Consultar o contato no banco de dados
                var contato = await _contatoQueryStore.ObterContatoPorIdAsync(request.Id);
                if (contato == null) throw new BusinessException("Contato não encontrado.");

                // Mapeia os dados do contato para o resultado
                var resultado = new ObterContatoPorIdResult
                {
                    Contato = new ContatoEntity(contato.Id, contato.Nome, contato.Telefone, contato.Email, contato.DDD)
                };

                return resultado;
            }
            catch (BusinessException ex)
            {
                _logger.LogWarning(ex, "Erro ao obter contato: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter contato: {Message}", ex.Message);
                throw new Exception("Erro ao obter contato.");
            }
        }

        public async Task<ObterContatosPorDddResult> ObterContatosPorDddQueueAsync(ObterContatosPorDddRequest request)
        {
            _logger.LogInformation("Iniciando a busca de contatos para o DDD: {Ddd}", request.Ddd);

            try
            {
                // Validação de entrada
                if (request.Ddd < 11 || request.Ddd > 99) throw new BusinessException("O DDD informado é inválido.");

                // Consultar os contatos no banco de dados com o DDD fornecido
                var contatos = await _contatoQueryStore.ObterContatosPorDddAsync(request.Ddd);

                if (contatos != null && contatos.Any()) return new ObterContatosPorDddResult(contatos.ToList());
                _logger.LogWarning("Nenhum contato encontrado para o DDD: {Ddd}", request.Ddd);
                return new ObterContatosPorDddResult(new List<ContatoEntity>());
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Erro ao obter contatos para o DDD: {Ddd}", request.Ddd);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter contatos para o DDD: {Ddd}", request.Ddd);
                throw new Exception("Erro ao obter contatos por DDD.");
            }
        }

        public async Task<AtualizarContatoResult> AtualizarContatoQueueAsync(AtualizarContatoRequest request)
        {
            _logger.LogInformation("Iniciando atualizacao de contato");
            try
            {
                // Validação de entrada
                ArgumentNullException.ThrowIfNull(request);

                // Criar entidade de contato
                var contato = new ContatoEntity(request.Id, request.Nome, request.Telefone, request.Email, request.DDD);

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
                _logger.LogError( $"Erro ao atualziar contato. {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
}
