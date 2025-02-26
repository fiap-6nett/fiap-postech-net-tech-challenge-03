using System.Text.Json.Nodes;
using Fiap.TechChallenge.Api.Create.Domain.DataBaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Fiap.TechChallenge.Api.Create.Infrastructure.Database;

public class ContatoQueryStore : IContatoQueryStore
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
    private readonly ILogger<ContatoQueryStore> _logger;


    public ContatoQueryStore(IDbContextFactory<AppDbContext> dbContextFactory, ILogger<ContatoQueryStore> logger)
    {
        _dbContextFactory = dbContextFactory;
        _logger = logger;
    }


    /// <summary>
    ///     Verifica se já existe um contato com o mesmo email ou telefone/DDD registrado para outro usuário.
    /// </summary>
    /// <param name="email">Email do contato.</param>
    /// <param name="telefone">Telefone do contato.</param>
    /// <param name="ddd">DDD do contato.</param>
    /// <param name="id">ID opcional do contato atual (para exclusão ao atualizar).</param>
    /// <returns>Retorna true se existir outro contato com o mesmo email ou telefone/DDD, false caso contrário.</returns>
    public async Task<bool> ContatoJaCadastradoAsync(string email, string telefone, int ddd, Guid? id = null)
    {
        try
        {
            await using var dbContext = _dbContextFactory.CreateDbContext(); // Cria o contexto sob demanda
            return await dbContext.Contatos.AnyAsync(c =>
                (c.Email == email || (c.Telefone == telefone && c.DDD == ddd)) &&
                (!id.HasValue || c.Id != id.Value));
        }
        catch (Exception ex)
        {
            var message = $"Erro ao verificar contato existente: {JsonConvert.SerializeObject(ex)}";
            _logger.LogError($"Erro ao verificar contato existente: {message}");
            throw new Exception(message);
        }
    }
}