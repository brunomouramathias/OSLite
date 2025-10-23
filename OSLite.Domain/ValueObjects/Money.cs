namespace OSLite.Domain.ValueObjects;

/// <summary>
/// Value Object que representa um valor monetário não negativo.
/// Implementado como record struct para garantir semântica de valor e imutabilidade.
/// </summary>
public readonly record struct Money
{
    public decimal Valor { get; }

    public Money(decimal valor)
    {
        if (valor < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(valor), "O valor monetário não pode ser negativo.");
        }

        Valor = valor;
    }

    public static Money operator +(Money a, Money b)
    {
        return new Money(a.Valor + b.Valor);
    }

    public static Money operator *(Money money, int quantidade)
    {
        return new Money(money.Valor * quantidade);
    }

    public override string ToString()
    {
        return Valor.ToString("C2");
    }
}

