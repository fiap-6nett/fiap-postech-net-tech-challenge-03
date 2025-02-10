namespace Fiap.TC03.Api.Cadastro.Infrastructure.Database;

public interface IContatoQueryStore
{
    /// <summary>
    ///     Verifica se já existe um contato com o mesmo email ou telefone/DDD registrado para outro usuário.
    /// </summary>
    /// <param name="email">Email do contato.</param>
    /// <param name="telefone">Telefone do contato.</param>
    /// <param name="ddd">DDD do contato.</param>
    /// <param name="id">ID opcional do contato atual (para exclusão ao atualizar).</param>
    /// <returns>Retorna true se existir outro contato com o mesmo email ou telefone/DDD, false caso contrário.</returns>
    Task<bool> ContatoJaCadastradoAsync(string email, string telefone, int ddd, Guid? id = null);
}