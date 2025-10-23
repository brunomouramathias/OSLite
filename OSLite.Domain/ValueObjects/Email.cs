using System.Text.RegularExpressions;

namespace OSLite.Domain.ValueObjects;

/// <summary>
/// Value Object que representa um endereço de e-mail válido.
/// Implementado como record para garantir semântica de valor e imutabilidade.
/// </summary>
public record Email
{
    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase
    );

    public string Endereco { get; }

    public Email(string endereco)
    {
        if (string.IsNullOrWhiteSpace(endereco))
        {
            throw new ArgumentException("O endereço de e-mail não pode ser vazio.", nameof(endereco));
        }

        if (!EmailRegex.IsMatch(endereco))
        {
            throw new ArgumentException("O formato do e-mail é inválido.", nameof(endereco));
        }

        Endereco = endereco;
    }

    public override string ToString()
    {
        return Endereco;
    }
}

