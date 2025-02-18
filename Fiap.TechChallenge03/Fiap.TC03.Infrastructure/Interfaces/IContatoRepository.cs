using Fiap.TC03.Infrastructure.Entities;

namespace Fiap.TC03.Infrastructure.Interfaces
{
    public interface IContatoRepository
    {
        void Delete(Guid id);
        ContatoEntity SearchById(Guid id);
    }
}
