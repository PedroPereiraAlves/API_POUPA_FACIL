# Use a imagem base do .NET SDK 7.0
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copie os arquivos de projeto e restaure as dependências
COPY *.csproj .
RUN dotnet restore

# Copie todos os arquivos e construa a aplicação
COPY . .
RUN dotnet publish -c Release -o out

# Use a imagem base do runtime do .NET 7.0
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app/out .

# Exponha a porta da aplicação
EXPOSE 80
ENTRYPOINT ["dotnet", "API_POUPA_FACIL.dll"]
