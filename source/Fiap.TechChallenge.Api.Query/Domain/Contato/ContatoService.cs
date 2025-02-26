using Fiap.TechChallenge.Api.Query.Domain.Contato.Request;
using Fiap.TechChallenge.Api.Query.Domain.DataBaseContext;
using Fiap.TechChallenge.Api.Query.Domain.Result;
using Fiap.TechChallenge.Api.Query.Infrastructure.Database;
using Fiap.TechChallenge.Foundation.Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace Fiap.TechChallenge.Api.Query.Domain.Contato;

public class ContatoService : IContatoService
{
    private readonly IContatoQueryStore _contatoQueryStore;
    private readonly ILogger<ContatoService> _logger;

    public ContatoService(ILogger<ContatoService> logger, IContatoQueryStore contatoQueryStore)
    {
        _logger = logger;
        _contatoQueryStore = contatoQueryStore;
    }


    public async Task<ObterContatoPorIdResult> ObterContatoPorIdAsync(ObterContatoPorIdRequest request)
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

    public async Task<ObterContatosPorDddResult> ObterContatosPorDddAsync(ObterContatosPorDddRequest request)
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
}