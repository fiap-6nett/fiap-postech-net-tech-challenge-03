using Fiap.TechChallenge.Core.Entities;

namespace Fiap.TechChallenge.Core.Data.CommandStores
{
    public interface IContatoCommandStore
    {
        /// <summary>
        /// Cria um novo contato no banco de dados.
        /// </summary>
        /// <param name="contato">A entidade de contato a ser criada.</param>
        /// <returns>
        /// True se o contato for criado com sucesso; caso contrário, false.
        /// </returns>
        Task<bool> CriarContatoAsync(ContatoEntity contato);

        /// <summary>
        /// Remove o contato com o ID informado do banco de dados.
        /// </summary>
        /// <param name="id">O identificador do contato a ser removido.</param>
        /// <returns>True se a exclusão for realizada com sucesso; caso contrário, false.</returns>
        Task<bool> RemoverContatoAsync(Guid id);
    }
}
