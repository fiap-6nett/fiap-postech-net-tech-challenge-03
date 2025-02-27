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

        public async Task<bool> RemoverContatoAsync(Guid id)
        {
            try
            {
                await using var dbContext = _dbContextFactory.CreateDbContext();
                var contato = await dbContext.Contatos.FindAsync(id);
                if (contato == null)
                {
                    _logger.LogWarning("Contato com ID {Id} não encontrado para exclusão definitiva.", id);
                    return false;
                }

                dbContext.Contatos.Remove(contato);
                await dbContext.SaveChangesAsync();
                _logger.LogInformation("Contato com ID {Id} removido definitivamente.", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover definitivamente o contato com ID {Id}.", id);
                throw;
            }
        }
    }
}
