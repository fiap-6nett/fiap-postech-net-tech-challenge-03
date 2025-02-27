using Fiap.TechChallenge.Foundation.Core.Domain;
using Fiap.TechChallenge.Foundation.Core.Extensions;

namespace Fiap.TechChallenge.Core.Entities
{
    /// <summary>
    ///     Representa a entidade Contato.
    ///     Um Contato é identificado de forma única por um ID e possui características como Nome, Telefone, Email e DDD.
    ///     A entidade Contato é responsável por manter o estado e integridade dos seus atributos.
    ///     Modificações no estado da entidade devem passar por validações para garantir a consistência.
    /// </summary>
    public sealed class ContatoEntity : EntityBase<Guid>
    {
        #region Properties

        public string Nome { get; private set; }
        public string Telefone { get; private set; }
        public string Email { get; private set; }
        public int DDD { get; private set; }

        #endregion

        #region Constructors

        // Construtor padrão necessário para o Entity Framework Core
        public ContatoEntity()
        {
        }

        /// <summary>
        ///     Construtor que recebe todos os parâmetros da entidade.
        /// </summary>
        public ContatoEntity(Guid id, string nome, string telefone, string email, int ddd)
        {
            SetId(id);
            Nome = nome;
            Telefone = telefone;
            Email = email;
            DDD = ddd;
        }

        /// <summary>
        ///     Construtor sem o ID, útil para criar novas instâncias sem ID gerado previamente.
        /// </summary>
        public ContatoEntity(string nome, string telefone, string email, int ddd)
        {
            Nome = nome;
            Telefone = telefone;
            Email = email;
            DDD = ddd;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Modifica o nome do contato, garantindo que não seja nulo ou vazio.
        /// </summary>
        /// <param name="nome">Novo nome do contato.</param>
        public void SetNome(string nome)
        {
            TechChallenge.Foundation.Core.Validations.Contract.Requires.IsNotNullOrWhiteSpace(nome, nameof(nome)).Guard();
            Nome = nome;
        }

        /// <summary>
        ///     Modifica o telefone do contato, garantindo que não seja nulo ou vazio.
        /// </summary>
        /// <param name="telefone">Novo telefone do contato.</param>
        public void SetTelefone(string telefone)
        {
            TechChallenge.Foundation.Core.Validations.Contract.Requires.IsNotNullOrWhiteSpace(telefone, nameof(telefone)).Guard();
            Telefone = telefone;
        }

        /// <summary>
        ///     Modifica o email do contato, garantindo que seja um formato válido e não seja nulo ou vazio.
        /// </summary>
        /// <param name="email">Novo email do contato.</param>
        public void SetEmail(string email)
        {
            TechChallenge.Foundation.Core.Validations.Contract.Requires.IsNotNullOrWhiteSpace(email, nameof(email)).IsValidEmail(email, nameof(email)).Guard();
            Email = email;
        }

        /// <summary>
        ///     Modifica o DDD (Discagem Direta à Distância) do contato, garantindo que o DDD seja um número válido entre 11 e 99.
        /// </summary>
        /// <param name="ddd">Novo DDD do contato.</param>
        public void SetDDD(int ddd)
        {
            TechChallenge.Foundation.Core.Validations.Contract.Requires.IsBetween(ddd, 11, 99, nameof(ddd)).Guard();
            DDD = ddd;
        }

        #endregion
    }
}
