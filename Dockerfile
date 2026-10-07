FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY API_POUPA_FACIL.csproj ./
RUN dotnet restore API_POUPA_FACIL.csproj

COPY . .
RUN dotnet publish API_POUPA_FACIL.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "API_POUPA_FACIL.dll"]
