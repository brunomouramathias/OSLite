using OSLite.Domain.Entities;
using OSLite.Domain.Enums;
using OSLite.Domain.ValueObjects;
using Xunit;

namespace OSLite.Domain.Tests;

public class ClienteTests
{
    [Fact]
    public void Cliente_adiciona_ordem_sincroniza_cliente_na_ordem()
    {
        // Arrange
        var cliente = new Cliente(1, "João Silva", new Email("joao@email.com"), "11999999999");
        var ordem = new OrdemDeServico(1, cliente.Id);

        // Act
        cliente.AdicionarOrdem(ordem);

        // Assert
        Assert.Contains(ordem, cliente.Ordens);
        Assert.Equal(cliente, ordem.Cliente);
        Assert.Equal(cliente.Id, ordem.ClienteId);
    }

    [Fact]
    public void OS_trocar_de_cliente_atualiza_colecoes_dos_clientes()
    {
        // Arrange
        var cliente1 = new Cliente(1, "João Silva", new Email("joao@email.com"), "11111111111");
        var cliente2 = new Cliente(2, "Maria Santos", new Email("maria@email.com"), "22222222222");
        var ordem = new OrdemDeServico(1, cliente1.Id);
        
        cliente1.AdicionarOrdem(ordem);

        // Act - Trocar para o segundo cliente
        cliente1.RemoverOrdem(ordem);
        cliente2.AdicionarOrdem(ordem);

        // Assert
        Assert.DoesNotContain(ordem, cliente1.Ordens);
        Assert.Contains(ordem, cliente2.Ordens);
        Assert.Equal(cliente2, ordem.Cliente);
        Assert.Equal(cliente2.Id, ordem.ClienteId);
    }

    [Fact]
    public void Cliente_remove_ordem_dessincroniza_referencia()
    {
        // Arrange
        var cliente = new Cliente(1, "João Silva", new Email("joao@email.com"), "11999999999");
        var ordem = new OrdemDeServico(1, cliente.Id);
        cliente.AdicionarOrdem(ordem);

        // Act
        cliente.RemoverOrdem(ordem);

        // Assert
        Assert.DoesNotContain(ordem, cliente.Ordens);
    }

    [Fact]
    public void Cliente_criado_sem_ordens()
    {
        // Arrange & Act
        var cliente = new Cliente(1, "João Silva", new Email("joao@email.com"), "11999999999");

        // Assert
        Assert.Empty(cliente.Ordens);
    }

    [Fact]
    public void Cliente_rejeita_nome_vazio()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new Cliente(1, "", new Email("email@test.com"), "11111111111")
        );
    }

    [Fact]
    public void Cliente_rejeita_nome_null()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() =>
            new Cliente(1, null!, new Email("email@test.com"), "11111111111")
        );
    }

    [Fact]
    public void Cliente_aceita_telefone_valido()
    {
        // Arrange & Act
        var cliente = new Cliente(1, "João Silva", new Email("joao@email.com"), "11999999999");

        // Assert
        Assert.Equal("11999999999", cliente.Telefone);
    }

    [Fact]
    public void Cliente_pode_ter_multiplas_ordens()
    {
        // Arrange
        var cliente = new Cliente(1, "João Silva", new Email("joao@email.com"), "11999999999");
        var ordem1 = new OrdemDeServico(1, cliente.Id);
        var ordem2 = new OrdemDeServico(2, cliente.Id);

        // Act
        cliente.AdicionarOrdem(ordem1);
        cliente.AdicionarOrdem(ordem2);

        // Assert
        Assert.Equal(2, cliente.Ordens.Count);
        Assert.Contains(ordem1, cliente.Ordens);
        Assert.Contains(ordem2, cliente.Ordens);
    }

    [Fact]
    public void Cliente_nao_adiciona_ordem_duplicada()
    {
        // Arrange
        var cliente = new Cliente(1, "João Silva", new Email("joao@email.com"), "11999999999");
        var ordem = new OrdemDeServico(1, cliente.Id);
        cliente.AdicionarOrdem(ordem);

        // Act
        cliente.AdicionarOrdem(ordem); // Tenta adicionar novamente

        // Assert
        Assert.Single(cliente.Ordens); // Deve ter apenas 1
    }
}

