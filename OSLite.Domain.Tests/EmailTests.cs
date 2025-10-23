using OSLite.Domain.ValueObjects;
using Xunit;

namespace OSLite.Domain.Tests;

public class EmailTests
{
    [Fact]
    public void Email_invalido_deve_falhar()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => new Email("emailinvalido"));
    }

    [Fact]
    public void Email_vazio_deve_falhar()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => new Email(""));
    }

    [Fact]
    public void Email_null_deve_falhar()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => new Email(null!));
    }

    [Fact]
    public void Email_valido_deve_ser_aceito()
    {
        // Arrange & Act
        var email = new Email("cliente@exemplo.com");

        // Assert
        Assert.Equal("cliente@exemplo.com", email.Endereco);
    }

    [Fact]
    public void Emails_com_mesmo_endereco_sao_iguais()
    {
        // Arrange
        var email1 = new Email("teste@teste.com");
        var email2 = new Email("teste@teste.com");

        // Assert
        Assert.Equal(email1, email2);
    }

    [Theory]
    [InlineData("usuario@dominio.com")]
    [InlineData("nome.sobrenome@empresa.com.br")]
    [InlineData("teste123@test.org")]
    public void Email_aceita_formatos_validos(string endereco)
    {
        // Arrange & Act
        var email = new Email(endereco);

        // Assert
        Assert.Equal(endereco, email.Endereco);
    }
}

