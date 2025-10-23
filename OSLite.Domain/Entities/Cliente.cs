using OSLite.Domain.ValueObjects;

namespace OSLite.Domain.Entities;

/// <summary>
/// Representa um cliente da assistência técnica.
/// É uma entidade com identidade (Id) e mantém navegabilidade bidirecional com OrdemDeServico.
/// </summary>
public class Cliente
{
    private readonly List<OrdemDeServico> _ordens = new();

    public int Id { get; }
    public string Nome { get; }
    public Email Email { get; }
    public string Telefone { get; }

    /// <summary>
    /// Coleção somente leitura das ordens de serviço do cliente.
    /// </summary>
    public IReadOnlyCollection<OrdemDeServico> Ordens => _ordens.AsReadOnly();

    public Cliente(int id, string nome, Email email, string telefone)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            throw new ArgumentException("O nome do cliente não pode ser vazio.", nameof(nome));
        }

        Id = id;
        Nome = nome;
        Email = email;
        Telefone = telefone;
    }

    /// <summary>
    /// Adiciona uma ordem de serviço ao cliente.
    /// Garante a navegabilidade bidirecional sincronizando a referência na OS.
    /// </summary>
    public void AdicionarOrdem(OrdemDeServico ordem)
    {
        if (ordem == null)
        {
            throw new ArgumentNullException(nameof(ordem));
        }

        // Evita duplicação
        if (_ordens.Contains(ordem))
        {
            return;
        }

        _ordens.Add(ordem);
        ordem.DefinirCliente(this); // Sincroniza o outro lado da navegabilidade
    }

    /// <summary>
    /// Remove uma ordem de serviço do cliente.
    /// Mantém a consistência da navegabilidade bidirecional.
    /// </summary>
    public void RemoverOrdem(OrdemDeServico ordem)
    {
        if (ordem == null)
        {
            throw new ArgumentNullException(nameof(ordem));
        }

        _ordens.Remove(ordem);
    }
}

