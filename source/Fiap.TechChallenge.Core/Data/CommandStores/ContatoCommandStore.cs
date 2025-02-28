using Fiap.TechChallenge.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Fiap.TechChallenge.Core.Data.CommandStores
{
    public class ContatoCommandStore : IContatoCommandStore
    {
        private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
        private readonly ILogger<ContatoCommandStore> _logger;

        public ContatoCommandStore(IDbContextFactory<AppDbContext> dbContextFactory, ILogger<ContatoCommandStore> logger)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
        }

        /// <summary>
        /// Cria um novo contato no banco de dados.
        /// </summary>
        /// <param name="contato">A entidade de contato a ser criada.</param>
        /// <returns>True se o contato for criado com sucesso; caso contrário, false.</returns>
        public async Task<bool> CriarContatoAsync(ContatoEntity contato)
        {
            try
            {
                await using var dbContext = _dbContextFactory.CreateDbContext();
                await dbContext.Contatos.AddAsync(contato);
                var changes = await dbContext.SaveChangesAsync();

                if (changes > 0)
                {
                    _logger.LogInformation("Contato com ID {Id} criado", contato.Id);
                    return true;
                }

                _logger.LogWarning("Nenhuma alteração realizada ao criar o contato com ID {Id}", contato.Id);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar o contato com ID {Id}", contato.Id);
                throw;
            }
        }

        /// <summary>
        /// Remove o contato do banco de dados.
        /// </summary>
        /// <param name="id">O identificador do contato a ser removido.</param>
        /// <returns>True se a exclusão for realizada com sucesso; caso contrário, false.</returns>
        public async Task<bool> RemoverContatoAsync(Guid id)
        {
            try
            {
                await using var dbContext = _dbContextFactory.CreateDbContext();
                var contato = await dbContext.Contatos.FindAsync(id);
                if (contato == null)
                {
                    _logger.LogWarning("Contato com ID {Id} não encontrado para exclusão.", id);
                    return false;
                }

                dbContext.Contatos.Remove(contato);
                await dbContext.SaveChangesAsync();
                _logger.LogInformation("Contato com ID {Id} removido.", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover o contato com ID {Id}.", id);
                throw;
            }
        }

        public async Task<bool> AtualizarContatoAsync(ContatoEntity contato)
        {
            try
            {
                await using var dbContext = _dbContextFactory.CreateDbContext();
                var contatoExistente = await dbContext.Contatos.FindAsync(contato.Id);
                if (contatoExistente == null)
                {
                    _logger.LogWarning("Contato com ID {Id} não encontrado para atualização.", contato.Id);
                    return false;
                }

                contatoExistente.Update(contato.Nome, contato.Telefone, contato.Email, contato.DDD);

                var changes = await dbContext.SaveChangesAsync();
                if (changes > 0)
                {
                    _logger.LogInformation("Contato com ID {Id} atualizado com sucesso.", contato.Id);
                    return true;
                }
                else
                {
                    _logger.LogWarning("Nenhuma alteração realizada na atualização do contato com ID {Id}.", contato.Id);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar o contato com ID {Id}.", contato.Id);
                throw;
            }
        }
    }
}
