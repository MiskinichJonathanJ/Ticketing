# Sistema de Ticketing

Este proyecto es una API profesional diseñada para la gestión y reserva de asientos para eventos, construida bajo una arquitectura limpia (Clean Architecture) y siguiendo patrones de diseño modernos como CQRS con MediatR.

## 🚀 Tecnologías Utilizadas

- **Runtime:** .NET 8
- **Base de Datos:** SQL Server (Entity Framework Core)
- **Arquitectura:** Clean Architecture
- **Patrones:** CQRS (con MediatR), Repository, Unit of Work
- **Validación:** FluentValidation
- **Mapeo de Objetos:** AutoMapper
- **Documentación:** Swagger (OpenAPI)
- **Contenerización:** Docker & Docker Compose

## 📋 Requisitos Previos

Antes de comenzar, asegúrate de tener instalado lo siguiente:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (recomendado para la base de datos)
- [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms) o Azure Data Studio (opcional, para visualizar datos)

## 🛠️ Configuración e Instalación

### 1. Clonar el repositorio
```bash
git clone https://github.com/MiskinichJonathanJ/Ticketing.git
cd Ticketing
```

### 2. Levantar la Infraestructura (Base de Datos)
El proyecto incluye un archivo `docker-compose.yml` que levanta una instancia de SQL Server 2022.

```bash
docker-compose up -d
```

### 3. Configurar la Base de Datos
La aplicación está configurada para ejecutar las migraciones automáticamente al iniciar en el entorno de `Development`. Asegúrate de que la cadena de conexión en `appsettings.Development.json` o en las variables de entorno de Docker sea correcta.

**Cadena de conexión por defecto (Docker):**
`Server=localhost,1433;Database=TicketingDb;User Id=sa;Password=YourSecurePassword123!;TrustServerCertificate=True`

## 🏃 Cómo Ejecutar la Aplicación

### Opción A: Usando .NET CLI (Local)
Desde la raíz del proyecto o la carpeta de la API:

```bash
cd Ticketing
dotnet run
```
La API estará disponible en `https://localhost:5001` o `http://localhost:5000`.

### Opción B: Usando Docker (Full Stack)
Si prefieres correr todo el entorno (API + DB) en contenedores:

```bash
docker-compose up --build
```
La API estará disponible en `http://localhost:5000`.

## 📖 Documentación de la API
Una vez que la aplicación esté corriendo, puedes acceder a la interfaz de Swagger para probar los endpoints:

- URL: `http://localhost:5000/swagger/index.html`

## 📂 Estructura del Proyecto

- **Ticketing:** Capa de presentación (Web API). Contiene los controladores, middlewares y configuración de la aplicación.
- **Ticketing.Application:** Lógica de negocio, DTOs, interfaces de repositorios, casos de uso (Commands/Queries) y mapeos.
- **Ticketing.Domain:** Entidades principales, enums y excepciones de dominio. No tiene dependencias externas.
- **Ticketing.Infrastructure:** Implementación de persistencia (DbContext, Repositorios, Migraciones) y servicios externos.

## ✅ Funcionalidades Principales
- Listado de eventos.
- Consulta de sectores y asientos por evento.
- Reserva de asientos con validaciones concurrentes.
- Logs de auditoría automáticos.
