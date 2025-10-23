using OSLite.Domain.Enums;
using OSLite.Domain.ValueObjects;

namespace OSLite.Domain.Entities;

/// <summary>
/// Representa uma Ordem de Serviço de uma assistência técnica.
/// É uma entidade com identidade (Id) e comportamento complexo de transição de estados.
/// </summary>
public class OrdemDeServico
{
    private readonly List<ItemDeServico> _itens = new();

    public int Id { get; }
    public int ClienteId { get; private set; }
    public Cliente? Cliente { get; private set; }
    public DateOnly DataAbertura { get; }
    public StatusOS Status { get; private set; }

    /// <summary>
    /// Coleção somente leitura de itens de serviço.
    /// </summary>
    public IReadOnlyCollection<ItemDeServico> Itens => _itens.AsReadOnly();

    /// <summary>
    /// Total derivado da soma dos subtotais dos itens.
    /// Não é persistido, é sempre calculado.
    /// </summary>
    public Money Total
    {
        get
        {
            var total = 0m;
            foreach (var item in _itens)
            {
                total += item.CalcularSubtotal().Valor;
            }
            return new Money(total);
        }
    }

    public OrdemDeServico(int id, int clienteId)
    {
        Id = id;
        ClienteId = clienteId;
        DataAbertura = DateOnly.FromDateTime(DateTime.Now);
        Status = StatusOS.Aberta;
    }

    /// <summary>
    /// Define o cliente desta ordem de serviço (usado para navegabilidade bidirecional).
    /// </summary>
    internal void DefinirCliente(Cliente cliente)
    {
        Cliente = cliente;
        ClienteId = cliente.Id;
    }

    /// <summary>
    /// Adiciona um item à ordem de serviço.
    /// Só permite se a OS estiver em status Aberta ou EmExecucao.
    /// </summary>
    public void AdicionarItem(ItemDeServico item)
    {
        if (Status == StatusOS.Concluida || Status == StatusOS.Cancelada)
        {
            throw new InvalidOperationException(
                $"Não é possível adicionar itens a uma OS com status {Status}."
            );
        }

        _itens.Add(item);
    }

    /// <summary>
    /// Remove um item da ordem de serviço.
    /// Só permite se a OS estiver em status Aberta ou EmExecucao.
    /// </summary>
    public void RemoverItem(ItemDeServico item)
    {
        if (Status == StatusOS.Concluida || Status == StatusOS.Cancelada)
        {
            throw new InvalidOperationException(
                $"Não é possível remover itens de uma OS com status {Status}."
            );
        }

        _itens.Remove(item);
    }

    /// <summary>
    /// Inicia a execução da ordem de serviço.
    /// Transição: Aberta → EmExecucao
    /// Requer pelo menos 1 item.
    /// </summary>
    public void IniciarExecucao()
    {
        if (Status != StatusOS.Aberta)
        {
            throw new InvalidOperationException(
                $"Não é possível iniciar execução de uma OS com status {Status}."
            );
        }

        if (_itens.Count == 0)
        {
            throw new InvalidOperationException(
                "Não é possível iniciar execução de uma OS sem itens."
            );
        }

        Status = StatusOS.EmExecucao;
    }

    /// <summary>
    /// Conclui a ordem de serviço.
    /// Transição: EmExecucao → Concluida
    /// </summary>
    public void Concluir()
    {
        if (Status != StatusOS.EmExecucao)
        {
            throw new InvalidOperationException(
                $"Não é possível concluir uma OS com status {Status}."
            );
        }

        Status = StatusOS.Concluida;
    }

    /// <summary>
    /// Cancela a ordem de serviço.
    /// Transições permitidas: Aberta → Cancelada ou EmExecucao → Cancelada
    /// </summary>
    public void Cancelar()
    {
        if (Status == StatusOS.Concluida)
        {
            throw new InvalidOperationException(
                "Não é possível cancelar uma OS já concluída."
            );
        }

        if (Status == StatusOS.Cancelada)
        {
            throw new InvalidOperationException(
                "A OS já está cancelada."
            );
        }

        Status = StatusOS.Cancelada;
    }
}

