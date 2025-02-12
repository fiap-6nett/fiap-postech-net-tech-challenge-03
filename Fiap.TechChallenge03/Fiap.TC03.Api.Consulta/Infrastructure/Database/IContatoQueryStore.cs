using Fiap.TC03.Api.Consulta.Domain.DataBaseContext;

namespace Fiap.TC03.Api.Consulta.Infrastructure.Database;

public interface IContatoQueryStore
{
    /// <summary>
    ///     Obtém todos os contatos filtrados por DDD.
    /// </summary>
    /// <param name="ddd">O código DDD da região.</param>
    /// <returns>Uma lista de contatos associados ao DDD fornecido.</returns>
    Task<IEnumerable<ContatoEntity>> ObterContatosPorDddAsync(int ddd);

    /// <summary>
    ///     Obtém um contato específico pelo seu ID.
    /// </summary>
    /// <param name="id">O identificador único do contato.</param>
    /// <returns>O contato correspondente ao ID fornecido.</returns>
    Task<ContatoEntity?> ObterContatoPorIdAsync(Guid id);

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