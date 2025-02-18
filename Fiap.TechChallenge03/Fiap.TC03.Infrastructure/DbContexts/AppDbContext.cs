using Fiap.TC03.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fiap.TC03.Infrastructure.DbContexts
{
    public class AppDbContext : DbContext
    {
        #region Constructors

        public AppDbContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        { }

        #endregion

        public DbSet<ContatoEntity> Contatos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // TODO Adicione outras configurações de mapeamento, se necessário.
        }
    }
}
