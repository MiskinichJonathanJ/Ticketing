FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos .csproj y restaurar dependencias
COPY ["Ticketing/Ticketing.csproj", "Ticketing/"]
COPY ["Ticketing.Application/Ticketing.Application.csproj", "Ticketing.Application/"]
COPY ["Ticketing.Domain/Ticketing.Domain.csproj", "Ticketing.Domain/"]
COPY ["Ticketing.Infrastructure/Ticketing.Infrastructure.csproj", "Ticketing.Infrastructure/"]

RUN dotnet restore "Ticketing/Ticketing.csproj"

# Copiar el resto del código y compilar
COPY . .
WORKDIR "/src/Ticketing"
RUN dotnet build "Ticketing.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ticketing.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ticketing.dll"]
