# Instruções para agentes — AssistHub

## Contexto

AssistHub é um projeto de microsserviços em .NET 10. A solução principal é `AssistHub.sln`. Os serviços estão em `services/`: `IdentityService`, `ConversationService`, `IntegrationService`, `KnowledgeBaseService`, `AgentOrchestratorService` e `NotificationService`. Código reutilizável fica em `building-blocks/AssistHub.BuildingBlocks`.

## Arquitetura: Clean Architecture + Vertical Slice

- Use a separação de responsabilidades e a direção de dependências da Clean Architecture **dentro de cada serviço**: `Domain` não depende de API, Application ou Infrastructure; `Application` depende de `Domain`; `Infrastructure` implementa contratos internos; `Api` é a borda HTTP e o ponto de composição.
- Organize a implementação **por caso de uso**: `Application/Features/<Area>/<CasoDeUso>/` para regras, contratos e validação do caso; `Api/Features/<Area>/<CasoDeUso>/` para o endpoint e seus contratos HTTP. Modelos compartilhados da área podem ficar em `Domain/Features/<Area>/`; implementações específicas de banco ficam em `Infrastructure/Persistence/<Area>/`. Não espalhe a regra de um caso de uso por pastas horizontais genéricas de `Handlers`, `Services` ou `Repositories`.
- Configuração e clientes Mongo, índices, serializers e repositórios concretos pertencem a `Infrastructure`, nunca a `Api` ou `Domain`. Registre `Infrastructure` em `Program.cs` sem colocar regras de negócio ali. `building-blocks/` deve permanecer independente do Mongo e de outros detalhes de infraestrutura.
- Hoje só `IdentityService` precisa de `Domain`, `Application` e `Infrastructure`: há modelos em `Domain/Features/Users` e `Domain/Features/Tenants` e persistência em `Infrastructure/Persistence`. Ainda não há casos de uso ou endpoints implementados; `Application/Features` e `Api/Features` estão preparados para eles. Os outros cinco serviços continuam apenas com `Api/Features` vazio; crie projetos adicionais quando houver código que justifique a separação.
- Não introduza MediatR, interfaces ou outros projetos apenas para completar um diagrama; preserve a direção das dependências quando surgirem funcionalidades reais.

## Como trabalhar

- Leia os arquivos relacionados antes de alterar um serviço. Respeite seus limites: não acople diretamente um serviço à implementação interna de outro.
- Faça mudanças pequenas e verificáveis, sem criar abstrações, dependências ou infraestrutura que ainda não sejam necessárias.
- Preserve o comportamento existente ao reorganizar código; acompanhe mudanças de comportamento com testes apropriados.
- Registre decisões de arquitetura e milestones em `docs/` quando houver conteúdo real a documentar. Separe o que foi concluído do que está planejado.
- Nunca inclua `Co-Authored-By` nem links de sessões de ferramentas nas mensagens de commit.

## Validação

- Execute `dotnet build AssistHub.sln` após alterações de código e `dotnet test AssistHub.sln` quando houver testes aplicáveis.
- Se o SDK .NET 10 não estiver disponível, informe que a validação não foi executada; não a declare como aprovada.
- Revise o diff para evitar segredos, artefatos gerados e alterações fora do escopo.
