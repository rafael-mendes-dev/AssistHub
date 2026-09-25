# Instruções para agentes — AssistHub

## Contexto

AssistHub é um projeto de microsserviços em .NET 10. A solução principal é `AssistHub.sln`. Os serviços estão em `services/`: `IdentityService`, `ConversationService`, `IntegrationService`, `KnowledgeBaseService`, `AgentOrchestratorService` e `NotificationService`. Código reutilizável fica em `building-blocks/AssistHub.BuildingBlocks`.

## Como trabalhar

- Leia os arquivos relacionados antes de alterar um serviço. Respeite seus limites: não acople diretamente um serviço à implementação interna de outro.
- Faça mudanças pequenas e verificáveis, sem criar abstrações, dependências ou infraestrutura que ainda não sejam necessárias.
- Preserve o comportamento existente ao reorganizar código; acompanhe mudanças de comportamento com testes apropriados.
- Não apresente funcionalidades planejadas como implementadas. A organização atual por projetos `Api`, `Application`, `Domain` e `Infrastructure` ainda será migrada para Vertical Slice; atualize este arquivo com o padrão concreto quando a migração for feita.
- Registre decisões de arquitetura e milestones em `docs/` quando houver conteúdo real a documentar. Separe o que foi concluído do que está planejado.
- Nunca inclua `Co-Authored-By` nem links de sessões de ferramentas nas mensagens de commit.

## Validação

- Execute `dotnet build AssistHub.sln` após alterações de código e `dotnet test AssistHub.sln` quando houver testes aplicáveis.
- Se o SDK .NET 10 não estiver disponível, informe que a validação não foi executada; não a declare como aprovada.
- Revise o diff para evitar segredos, artefatos gerados e alterações fora do escopo.
