FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["bolada.api/bolada.api.csproj", "bolada.api/"]
RUN dotnet restore "bolada.api/bolada.api.csproj"
COPY . .
WORKDIR "/src/bolada.api"
RUN dotnet build "bolada.api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "bolada.api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "bolada.api.dll"]