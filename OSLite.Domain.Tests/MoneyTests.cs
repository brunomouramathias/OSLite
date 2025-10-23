using OSLite.Domain.ValueObjects;
using Xunit;

namespace OSLite.Domain.Tests;

public class MoneyTests
{
    [Fact]
    public void Money_nao_aceita_negativo()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new Money(-10));
    }

    [Fact]
    public void Money_aceita_zero()
    {
        // Arrange & Act
        var money = new Money(0);

        // Assert
        Assert.Equal(0, money.Valor);
    }

    [Fact]
    public void Money_aceita_valor_positivo()
    {
        // Arrange & Act
        var money = new Money(100.50m);

        // Assert
        Assert.Equal(100.50m, money.Valor);
    }

    [Fact]
    public void Money_com_mesmo_valor_sao_iguais()
    {
        // Arrange
        var money1 = new Money(50);
        var money2 = new Money(50);

        // Assert
        Assert.Equal(money1, money2);
    }

    [Fact]
    public void Money_pode_ser_multiplicado()
    {
        // Arrange
        var money = new Money(10);

        // Act
        var resultado = money * 3;

        // Assert
        Assert.Equal(new Money(30), resultado);
    }

    [Fact]
    public void Money_pode_ser_somado()
    {
        // Arrange
        var money1 = new Money(10);
        var money2 = new Money(20);

        // Act
        var resultado = money1 + money2;

        // Assert
        Assert.Equal(new Money(30), resultado);
    }
}

