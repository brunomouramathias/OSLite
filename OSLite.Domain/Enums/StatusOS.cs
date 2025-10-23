namespace OSLite.Domain.Enums;

/// <summary>
/// Enum que representa os possíveis estados de uma Ordem de Serviço.
/// Facilita validações de transição de estado e evita strings mágicas.
/// </summary>
public enum StatusOS
{
    Aberta,
    EmExecucao,
    Concluida,
    Cancelada
}

