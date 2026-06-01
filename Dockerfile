FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["EdgeNetworkApi/EdgeNetworkApi.csproj", "EdgeNetworkApi/"]
COPY ["EdgeNetworkApplication/EdgeNetworkApplication.csproj", "EdgeNetworkApplication/"]
COPY ["EdgeNetworkDomain/EdgeNetworkDomain.csproj", "EdgeNetworkDomain/"]
COPY ["EdgeNetworkInfrastructure/EdgeNetworkInfrastructure.csproj", "EdgeNetworkInfrastructure/"]
RUN dotnet restore "EdgeNetworkApi/EdgeNetworkApi.csproj"
COPY . .
WORKDIR "/src/EdgeNetworkApi"
RUN dotnet build "EdgeNetworkApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EdgeNetworkApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EdgeNetworkApi.dll"]
