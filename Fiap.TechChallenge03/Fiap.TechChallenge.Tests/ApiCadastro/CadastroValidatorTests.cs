using Fiap.TC03.Api.Cadastro.Domain.Contract.CriarContato;
using FluentValidation.TestHelper;

namespace Fiap.TC03.Test.ApiCadastro;

public class CadastroValidatorTests
{
    private readonly CriarContatoCommandValidator _validator;

    public CadastroValidatorTests()
    {
        _validator = new CriarContatoCommandValidator();
    }
    
    // Testes para o Nome
    [Fact]
    public void Deve_Falhar_Quando_Nome_For_Vazio()
    {
        // Arrange
        var command = new CriarContatoCommand { Nome = string.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Nome).WithErrorMessage("O nome do contato é obrigatório.");
    }

    [Fact]
    public void Deve_Falhar_Quando_Nome_For_Menor_Que_Dois_Caracteres()
    {
        // Arrange
        var command = new CriarContatoCommand { Nome = "A" }; // Nome com menos de 2 caracteres

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Nome).WithErrorMessage("O nome deve ter no mínimo 2 caracteres.");
    }

    [Fact]
    public void Deve_Falhar_Quando_Nome_For_Maior_Que_100_Caracteres()
    {
        // Arrange
        var command = new CriarContatoCommand { Nome = new string('A', 101) }; // Nome com mais de 100 caracteres

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Nome).WithErrorMessage("O nome pode ter no máximo 100 caracteres.");
    }

    [Fact]
    public void Deve_Passar_Quando_Nome_For_Valido()
    {
        // Arrange
        var command = new CriarContatoCommand { Nome = "João" }; // Nome válido

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Nome);
    }

    // Testes para o Telefone
    [Fact]
    public void Deve_Falhar_Quando_Telefone_For_Vazio()
    {
        // Arrange
        var command = new CriarContatoCommand { Telefone = string.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Telefone).WithErrorMessage("O telefone do contato é obrigatório.");
    }

    [Fact]
    public void Deve_Falhar_Quando_Telefone_Nao_Tiver_Entre_8_E_15_Digitos()
    {
        // Arrange
        var command = new CriarContatoCommand { Telefone = "123456" }; // Telefone com menos de 8 dígitos

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Telefone)
            .WithErrorMessage("O telefone deve ter entre 8 e 15 dígitos.");
    }

    [Fact]
    public void Deve_Passar_Quando_Telefone_For_Valido()
    {
        // Arrange
        var command = new CriarContatoCommand { Telefone = "123456789" }; // Telefone válidoo

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Telefone);
    }

    // Testes para o Email
    [Fact]
    public void Deve_Falhar_Quando_Email_For_Vazio()
    {
        // Arrange
        var command = new CriarContatoCommand { Email = string.Empty };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage("O e-mail do contato é obrigatório.");
    }

    [Fact]
    public void Deve_Falhar_Quando_Email_For_Invalido()
    {
        // Arrange
        var command = new CriarContatoCommand { Email = "email_invalido" }; // Email inválido

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email).WithErrorMessage("O e-mail fornecido não é válido.");
    }

    [Fact]
    public void Deve_Passar_Quando_Email_For_Valido()
    {
        // Arrange
        var command = new CriarContatoCommand { Email = "joao@exemplo.com" }; // Email válido

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }

    // Testes para o DDD
    [Fact]
    public void Deve_Falhar_Quando_DDD_For_Menor_Que_11()
    {
        // Arrange
        var command = new CriarContatoCommand { DDD = 10 }; // DDD menor que o mínimo permitido

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DDD)
            .WithErrorMessage("O DDD deve ser composto por dois dígitos entre 11 e 99.");
    }

    [Fact]
    public void Deve_Falhar_Quando_DDD_For_Maior_Que_99()
    {
        // Arrange
        var command = new CriarContatoCommand { DDD = 100 }; // DDD maior que o máximo permitido

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.DDD)
            .WithErrorMessage("O DDD deve ser composto por dois dígitos entre 11 e 99.");
    }

    [Fact]
    public void Deve_Passar_Quando_DDD_For_Valido()
    {
        // Arrange
        var command = new CriarContatoCommand { DDD = 11 }; // DDD válido

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.DDD);
    }
}