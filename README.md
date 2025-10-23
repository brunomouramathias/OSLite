# OS-Lite - Sistema de Ordem de Serviço

Projeto desenvolvido como trabalho prático de Programação Orientada a Objetos, implementando um mini-domínio de ordens de serviço para assistência técnica.

## 📋 Sobre o Projeto

O OS-Lite é um sistema que permite gerenciar ordens de serviço de uma assistência técnica, com foco em modelagem de domínio, proteção de invariantes e desenvolvimento orientado por testes (TDD).

## 🏗️ Arquitetura do Projeto

```
OSLite/
├── OSLite.Domain/              # Biblioteca de classes com a lógica de domínio
│   ├── Entities/               # Entidades do domínio
│   │   ├── Cliente.cs
│   │   ├── OrdemDeServico.cs
│   │   └── ItemDeServico.cs
│   ├── ValueObjects/           # Objetos de valor
│   │   ├── Money.cs
│   │   └── Email.cs
│   ├── Enums/                  # Enumerações
│   │   ├── StatusOS.cs
│   │   └── CategoriaItem.cs
│   └── Exceptions/             # Exceções customizadas
│       └── DomainException.cs
│
└── OSLite.Domain.Tests/        # Testes unitários (xUnit)
    ├── ClienteTests.cs
    ├── OrdemDeServicoTests.cs
    ├── ItemDeServicoTests.cs
    ├── MoneyTests.cs
    └── EmailTests.cs
```

## 🎯 Decisões de Modelagem

### Classes vs Records vs Record Structs

#### Entidades (Classes)
- **Cliente**, **OrdemDeServico**: Implementadas como **classes** porque possuem **identidade própria**.
  - Dois clientes com o mesmo nome não são o mesmo cliente (são diferenciados pelo `Id`).
  - A identidade persiste mesmo que suas propriedades mudem.
  - Classes permitem encapsular comportamento complexo e manter estado mutável controlado.

#### Value Objects (Records/Record Structs)
- **Money** (record struct): Implementado como **record struct** porque:
  - Representa um valor monetário sem identidade própria.
  - Dois objetos Money com o mesmo valor são **iguais** (semântica de valor).
  - É pequeno (apenas um decimal) e se beneficia de alocação na stack.
  - Imutável por design, evitando efeitos colaterais.
  
- **Email** (record): Implementado como **record** porque:
  - Representa um endereço de e-mail sem identidade.
  - Dois e-mails com o mesmo endereço são iguais.
  - Records fornecem igualdade por valor automaticamente.
  - Imutabilidade garante que um e-mail válido nunca seja corrompido.

#### Enumerações (Enums)
- **StatusOS**, **CategoriaItem**: Implementados como **enums** porque:
  - Representam um conjunto fixo e limitado de valores possíveis.
  - Evitam "strings mágicas" e erros de digitação.
  - Permitem validação em tempo de compilação.
  - Tornam as transições de estado explícitas e verificáveis.

### Associações e Navegabilidade

#### Cliente ↔ OrdemDeServico (1:N Bidirecional)
- Um `Cliente` pode ter várias `OrdemDeServico` (1:N).
- Cada `OrdemDeServico` pertence a exatamente um `Cliente`.
- **Navegabilidade bidirecional**: a partir do Cliente, posso acessar suas ordens; a partir da Ordem, posso acessar o cliente.
- **Consistência garantida**: ao adicionar uma ordem ao cliente, automaticamente a ordem é sincronizada com a referência ao cliente.
- Método `DefinirCliente()` é **internal** para evitar quebra de encapsulamento.

#### OrdemDeServico → ItemDeServico (1:N Composição)
- Uma `OrdemDeServico` contém vários `ItemDeServico` (1:N).
- **Composição**: os itens nascem e morrem com a ordem.
- A lista `_itens` é privada; o acesso externo é via `IReadOnlyCollection`.
- Mutações controladas pelos métodos `AdicionarItem()` e `RemoverItem()`.

## 🔒 Proteção de Invariantes (Fail-Fast)

O projeto implementa validações rigorosas que falham imediatamente ao detectar estados inválidos:

### Money
- Não aceita valores negativos → `ArgumentOutOfRangeException`

### Email
- Não aceita formato inválido → `ArgumentException`
- Validação com Regex

### ItemDeServico
- Quantidade deve ser > 0 → `ArgumentOutOfRangeException`
- Descrição não pode ser vazia → `ArgumentException`

### OrdemDeServico
- **Transições de status rigorosas**:
  - `Aberta → EmExecucao`: requer pelo menos 1 item
  - `EmExecucao → Concluida`: permitido
  - `Aberta/EmExecucao → Cancelada`: permitido
  - `Concluida → Cancelada`: **bloqueado**
- **Bloqueios em estados finais**: Não permite adicionar/remover itens em ordens `Concluida` ou `Cancelada`
- **Total derivado**: calculado dinamicamente, sem persistência redundante

### Cliente
- Nome não pode ser vazio → `ArgumentException`
- Email deve ser válido (validado pelo VO Email)

## ✅ Cobertura de Testes (TDD)

O projeto foi desenvolvido seguindo **Test-Driven Development (TDD)**:

### Total de Testes: **47 testes passando**

- **MoneyTests**: 6 testes
  - Validação de valores negativos
  - Operações matemáticas (soma, multiplicação)
  - Igualdade por valor

- **EmailTests**: 8 testes
  - Validação de formato
  - Rejeição de valores inválidos

- **ItemDeServicoTests**: 8 testes
  - Criação válida e cálculo de subtotal
  - Validações de quantidade e descrição
  - Categoria opcional

- **OrdemDeServicoTests**: 16 testes
  - Transições de status (casos felizes e de falha)
  - Total derivado
  - Bloqueios em estados finais
  - Adição/remoção de itens

- **ClienteTests**: 9 testes
  - Navegabilidade bidirecional
  - Troca de cliente em ordem
  - Validações de nome

## 🚀 Como Executar

### Pré-requisitos
- .NET 9.0 SDK ou superior
- Visual Studio 2022 / VS Code / Rider

### Executar os Testes
```bash
# Na raiz do projeto
dotnet test

# Ou apenas compilar
dotnet build
```

### Resultados Esperados
```
Resumo do teste: total: 47; falhou: 0; bem-sucedido: 47; ignorado: 0
```

## 📝 Reflexão: Impacto de Records/Structs e Enums no Desenvolvimento

O uso de **records** e **record structs** para Value Objects trouxe clareza e segurança ao código. A semântica de valor é fundamental para objetos como `Money` e `Email`: não faz sentido que duas instâncias com o mesmo valor sejam consideradas diferentes. A imutabilidade automática dos records elimina uma classe inteira de bugs relacionados a mutações indesejadas. Além disso, a igualdade estrutural facilita enormemente os testes, pois posso comparar objetos diretamente sem implementar `Equals()` manualmente.

Os **enums** foram essenciais para tornar as regras de negócio explícitas. Ao usar `StatusOS` em vez de strings, as transições de estado ficaram verificáveis em tempo de compilação, e o código se tornou mais legível: é muito mais claro ler `if (Status == StatusOS.Concluida)` do que `if (status == "CONCLUIDA")`. Isso também facilitou a implementação das validações de transição, pois o compilador garante que todos os casos sejam tratados.

A abordagem **fail-fast** com invariantes protegidos foi crucial no TDD. Cada teste de falha tinha um objetivo claro: verificar que uma regra específica estava sendo protegida. Isso reduziu ambiguidades e tornou o código mais previsível. Por exemplo, saber que `Money` sempre será >= 0 permite que o resto do código assuma essa garantia sem verificações redundantes.

A **navegabilidade bidirecional** entre `Cliente` e `OrdemDeServico` exigiu atenção especial ao sincronismo. Centralizar o vínculo no método `AdicionarOrdem()` garantiu que ambos os lados permanecessem consistentes. O uso de coleções somente leitura (`IReadOnlyCollection`) evitou que código externo quebrasse as invariantes, e o método `DefinirCliente()` internal manteve o encapsulamento. Esse cuidado preveniu estados inválidos onde, por exemplo, uma ordem "achasse" que pertence a um cliente, mas esse cliente não tivesse a ordem em sua lista.

## 🎓 Conceitos Aplicados

- ✅ Classes para entidades (identidade)
- ✅ Records/Record Structs para value objects (semântica de valor)
- ✅ Enums para estados e categorias
- ✅ Associações 1:N (sem N:M)
- ✅ Navegabilidade bidirecional consistente
- ✅ Composição (OrdemDeServico → Itens)
- ✅ Invariantes protegidos com fail-fast
- ✅ Total derivado (sem persistência redundante)
- ✅ Test-Driven Development (TDD)
- ✅ Encapsulamento (coleções somente leitura)
- ❌ Sem herança ou polimorfismo (conforme restrição)
- ❌ Sem associações N:M (conforme restrição)

## 👨‍💻 Autor

Projeto desenvolvido como trabalho acadêmico de Programação Orientada a Objetos.

## 📄 Licença

Este projeto é de uso educacional.

