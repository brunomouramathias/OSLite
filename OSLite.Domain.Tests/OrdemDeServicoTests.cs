using OSLite.Domain.Entities;
using OSLite.Domain.Enums;
using OSLite.Domain.Exceptions;
using OSLite.Domain.ValueObjects;
using Xunit;

namespace OSLite.Domain.Tests;

public class OrdemDeServicoTests
{
    [Fact]
    public void OS_total_soma_subtotais_itens()
    {
        // Arrange
        var os = new OrdemDeServico(1, 1);
        os.AdicionarItem(new ItemDeServico("Item 1", 2, new Money(50)));
        os.AdicionarItem(new ItemDeServico("Item 2", 1, new Money(30)));

        // Act
        var total = os.Total;

        // Assert
        Assert.Equal(new Money(130), total);
    }

    [Fact]
    public void OS_criada_tem_status_aberta()
    {
        // Arrange & Act
        var os = new OrdemDeServico(1, 1);

        // Assert
        Assert.Equal(StatusOS.Aberta, os.Status);
    }

    [Fact]
    public void OS_aberta_inicia_execucao_quando_tem_itens()
    {
        // Arrange
        var os = new OrdemDeServico(1, 1);
        os.AdicionarItem(new ItemDeServico("Serviço", 1, new Money(100)));

        // Act
        os.IniciarExecucao();

        // Assert
        Assert.Equal(StatusOS.EmExecucao, os.Status);
    }

    [Fact]
    public void OS_aberta_nao_inicia_sem_itens()
    {
        // Arrange
        var os = new OrdemDeServico(1, 1);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => os.IniciarExecucao());
    }

    [Fact]
    public void OS_nao_adiciona_itens_em_concluida()
    {
        // Arrange
        var os = new OrdemDeServico(1, 1);
        os.AdicionarItem(new ItemDeServico("Serviço", 1, new Money(100)));
        os.IniciarExecucao();
        os.Concluir();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            os.AdicionarItem(new ItemDeServico("Outro", 1, new Money(50)))
        );
    }

    [Fact]
    public void OS_nao_adiciona_itens_em_cancelada()
    {
        // Arrange
        var os = new OrdemDeServico(1, 1);
        os.Cancelar();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            os.AdicionarItem(new ItemDeServico("Item", 1, new Money(50)))
        );
    }

    [Fact]
    public void OS_fluxo_aberta_para_execucao_para_concluida()
    {
        // Arrange
        var os = new OrdemDeServico(1, 1);
        os.AdicionarItem(new ItemDeServico("Reparo", 1, new Money(200)));

        // Act & Assert - Inicia execução
        os.IniciarExecucao();
        Assert.Equal(StatusOS.EmExecucao, os.Status);

        // Act & Assert - Conclui
        os.Concluir();
        Assert.Equal(StatusOS.Concluida, os.Status);
    }

    [Fact]
    public void OS_nao_conclui_se_nao_esta_em_execucao()
    {
        // Arrange
        var os = new OrdemDeServico(1, 1);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => os.Concluir());
    }

    [Fact]
    public void OS_pode_ser_cancelada_quando_aberta()
    {
        // Arrange
        var os = new OrdemDeServico(1, 1);

        // Act
        os.Cancelar();

        // Assert
        Assert.Equal(StatusOS.Cancelada, os.Status);
    }

    [Fact]
    public void OS_pode_ser_cancelada_quando_em_execucao()
    {
        // Arrange
        var os = new OrdemDeServico(1, 1);
        os.AdicionarItem(new ItemDeServico("Item", 1, new Money(100)));
        os.IniciarExecucao();

        // Act
        os.Cancelar();

        // Assert
        Assert.Equal(StatusOS.Cancelada, os.Status);
    }

    [Fact]
    public void OS_nao_pode_ser_cancelada_quando_concluida()
    {
        // Arrange
        var os = new OrdemDeServico(1, 1);
        os.AdicionarItem(new ItemDeServico("Item", 1, new Money(100)));
        os.IniciarExecucao();
        os.Concluir();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => os.Cancelar());
    }

    [Fact]
    public void OS_remove_item_quando_permitido()
    {
        // Arrange
        var os = new OrdemDeServico(1, 1);
        var item = new ItemDeServico("Item", 1, new Money(100));
        os.AdicionarItem(item);

        // Act
        os.RemoverItem(item);

        // Assert
        Assert.Equal(new Money(0), os.Total);
    }

    [Fact]
    public void OS_nao_remove_item_em_concluida()
    {
        // Arrange
        var os = new OrdemDeServico(1, 1);
        var item = new ItemDeServico("Item", 1, new Money(100));
        os.AdicionarItem(item);
        os.IniciarExecucao();
        os.Concluir();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => os.RemoverItem(item));
    }

    [Fact]
    public void OS_sem_itens_tem_total_zero()
    {
        // Arrange
        var os = new OrdemDeServico(1, 1);

        // Assert
        Assert.Equal(new Money(0), os.Total);
    }

    [Fact]
    public void OS_data_abertura_registrada_corretamente()
    {
        // Arrange
        var dataEsperada = DateOnly.FromDateTime(DateTime.Now);

        // Act
        var os = new OrdemDeServico(1, 1);

        // Assert
        Assert.Equal(dataEsperada, os.DataAbertura);
    }

    [Fact]
    public void OS_nao_pode_iniciar_execucao_se_ja_esta_em_execucao()
    {
        // Arrange
        var os = new OrdemDeServico(1, 1);
        os.AdicionarItem(new ItemDeServico("Item", 1, new Money(100)));
        os.IniciarExecucao();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => os.IniciarExecucao());
    }
}

