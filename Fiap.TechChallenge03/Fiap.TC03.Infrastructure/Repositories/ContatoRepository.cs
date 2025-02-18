using Fiap.TC03.Infrastructure.DbContexts;
using Fiap.TC03.Infrastructure.Entities;
using Fiap.TC03.Infrastructure.Interfaces;

namespace Fiap.TC03.Infrastructure.Repositories
{
    public class ContatoRepository : IContatoRepository
    {
        private readonly AppDbContext _appDbContext;

        public ContatoRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void Delete(Guid id)
        {
            _appDbContext.Contatos.Remove(SearchById(id));
            _appDbContext.SaveChanges();
        }

        public ContatoEntity SearchById(Guid id)
        {
            return _appDbContext.Contatos.FirstOrDefault(x => x.Id == id);
        }
    }
}
