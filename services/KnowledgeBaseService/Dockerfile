FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore services/KnowledgeBaseService/src/KnowledgeBaseService.Api/KnowledgeBaseService.Api.csproj
RUN dotnet publish services/KnowledgeBaseService/src/KnowledgeBaseService.Api/KnowledgeBaseService.Api.csproj \
    --configuration Release \
    --no-restore \
    --output /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080 \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "KnowledgeBaseService.Api.dll"]
