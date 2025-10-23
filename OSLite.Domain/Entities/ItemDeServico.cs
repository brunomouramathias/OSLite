using OSLite.Domain.Enums;
using OSLite.Domain.ValueObjects;

namespace OSLite.Domain.Entities;

/// <summary>
/// Representa um item de serviço que faz parte de uma Ordem de Serviço.
/// É uma parte da composição da OrdemDeServico (nasce e morre com ela).
/// </summary>
public class ItemDeServico
{
    public string Descricao { get; }
    public int Quantidade { get; }
    public Money PrecoUnitario { get; }
    public CategoriaItem? Categoria { get; }

    public ItemDeServico(string descricao, int quantidade, Money precoUnitario, CategoriaItem? categoria = null)
    {
        if (string.IsNullOrWhiteSpace(descricao))
        {
            throw new ArgumentException("A descrição do item não pode ser vazia.", nameof(descricao));
        }

        if (quantidade <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantidade), "A quantidade deve ser maior que zero.");
        }

        Descricao = descricao;
        Quantidade = quantidade;
        PrecoUnitario = precoUnitario;
        Categoria = categoria;
    }

    public Money CalcularSubtotal()
    {
        return PrecoUnitario * Quantidade;
    }
}

