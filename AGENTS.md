# Instruções para agentes — AssistHub

## Contexto

AssistHub é um projeto de microsserviços em .NET 10. A solução principal é `AssistHub.sln`. Os serviços estão em `services/`: `IdentityService`, `ConversationService`, `IntegrationService`, `KnowledgeBaseService`, `AgentOrchestratorService` e `NotificationService`. Código reutilizável fica em `building-blocks/AssistHub.BuildingBlocks`.

## Padrão de arquitetura: Vertical Slice

- Cada serviço tem um único projeto executável em `services/<Servico>/src/<Servico>.Api`. A solução raiz contém esses seis projetos, `AssistHub.BuildingBlocks` e os testes.
- Organize funcionalidades por caso de uso em `Features/<Area>/<CasoDeUso>/`. Mantenha juntos endpoint, contratos, validação, lógica e persistência exclusivos desse caso de uso, em vez de criar projetos horizontais `Application`, `Domain` e `Infrastructure`.
- Deixe em `Features/<Area>/` somente código realmente compartilhado pelos casos de uso daquela área. Reutilize `building-blocks/` apenas para mecanismos comuns a mais de um serviço; configuração de infraestrutura compartilhada dentro de um serviço pode ficar em `Persistence/`.
- `Program.cs` é o ponto de composição: registre dependências e endpoints dos slices ali, sem concentrar regras de negócio nesse arquivo. Não introduza MediatR ou outras abstrações sem necessidade concreta.
- No estado atual, apenas IdentityService contém modelos e persistência: estão agrupados em `Features/Users/`, `Features/Tenants/` e `Persistence/`. Ainda não existem endpoints ou casos de uso implementados; os demais serviços têm `Features/` preparado, sem funcionalidades inventadas.

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
