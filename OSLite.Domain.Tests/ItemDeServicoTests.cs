using OSLite.Domain.Entities;
using OSLite.Domain.Enums;
using OSLite.Domain.ValueObjects;
using Xunit;

namespace OSLite.Domain.Tests;

public class ItemDeServicoTests
{
    [Fact]
    public void ItemDeServico_cria_valido_e_calcula_subtotal()
    {
        // Arrange
        var descricao = "Troca de tela";
        var quantidade = 2;
        var precoUnitario = new Money(100);

        // Act
        var item = new ItemDeServico(descricao, quantidade, precoUnitario);

        // Assert
        Assert.Equal(descricao, item.Descricao);
        Assert.Equal(quantidade, item.Quantidade);
        Assert.Equal(precoUnitario, item.PrecoUnitario);
        Assert.Equal(new Money(200), item.CalcularSubtotal());
    }

    [Fact]
    public void ItemDeServico_rejeita_quantidade_zero()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new ItemDeServico("Descrição", 0, new Money(100))
        );
    }

    [Fact]
    public void ItemDeServico_rejeita_quantidade_negativa()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new ItemDeServico("Descrição", -1, new Money(100))
        );
    }

    [Fact]
    public void ItemDeServico_rejeita_descricao_vazia()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new ItemDeServico("", 1, new Money(100))
        );
    }

    [Fact]
    public void ItemDeServico_rejeita_descricao_null()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new ItemDeServico(null!, 1, new Money(100))
        );
    }

    [Fact]
    public void ItemDeServico_aceita_categoria()
    {
        // Arrange
        var item = new ItemDeServico("Diagnóstico inicial", 1, new Money(50), CategoriaItem.Diagnostico);

        // Assert
        Assert.Equal(CategoriaItem.Diagnostico, item.Categoria);
    }

    [Fact]
    public void ItemDeServico_sem_categoria_tem_null()
    {
        // Arrange
        var item = new ItemDeServico("Serviço", 1, new Money(50));

        // Assert
        Assert.Null(item.Categoria);
    }

    [Fact]
    public void ItemDeServico_calcula_subtotal_com_quantidade_multipla()
    {
        // Arrange
        var item = new ItemDeServico("Peça A", 5, new Money(25.50m));

        // Act
        var subtotal = item.CalcularSubtotal();

        // Assert
        Assert.Equal(new Money(127.50m), subtotal);
    }
}

