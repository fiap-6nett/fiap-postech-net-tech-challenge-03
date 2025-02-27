namespace Fiap.TechChallenge.Core.Data.CommandStores
{
    public interface IContatoCommandStore
    {
        /// <summary>
        /// Remove definitivamente o contato com o ID informado do banco de dados.
        /// </summary>
        /// <param name="id">O identificador do contato a ser removido.</param>
        /// <returns>True se a exclusão for realizada com sucesso; caso contrário, false.</returns>
        Task<bool> RemoverContatoAsync(Guid id);
    }
}
