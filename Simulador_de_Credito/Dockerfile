FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Simulador_de_Credito/Simulador_de_Credito.csproj", "Simulador_de_Credito/"]
COPY ["Simulador_de_Credito.Tests/Simulador_de_Credito.Tests.csproj", "Simulador_de_Credito.Tests/"]

RUN dotnet restore "Simulador_de_Credito/Simulador_de_Credito.csproj"
RUN dotnet restore "Simulador_de_Credito.Tests/Simulador_de_Credito.Tests.csproj"

COPY . .

WORKDIR "/src/Simulador_de_Credito"
RUN dotnet build "Simulador_de_Credito.csproj" -c $BUILD_CONFIGURATION -o /app/build

WORKDIR "/src/Simulador_de_Credito.Tests"
RUN dotnet test "Simulador_de_Credito.Tests.csproj" -c $BUILD_CONFIGURATION

WORKDIR "/src/Simulador_de_Credito"
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Simulador_de_Credito.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

EXPOSE 8080
EXPOSE 8081

RUN mkdir -p /app/logs && chmod 777 /app/logs

COPY --from=publish /app/publish .

ENV TZ=America/Sao_Paulo
ENTRYPOINT ["dotnet", "Simulador_de_Credito.dll"]