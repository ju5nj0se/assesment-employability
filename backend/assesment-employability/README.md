# Assessment Employability - Backend API

Esta es la API del proyecto Assessment Employability, construida con **.NET 8** siguiendo los principios de **Clean Architecture**.

## 🏗️ Arquitectura del Proyecto

El proyecto está dividido en 4 capas principales:

1.  **Domain**: Contiene las entidades base (`Course`, `Lesson`, `User`, `Status`), excepciones de dominio y lógica central libre de dependencias externas.
2.  **Application**: Define las interfaces (`Interfaces`), los objetos de transferencia de datos (`DTOs`), el mapeo de lógica y los servicios de aplicación (`Services`). Aquí reside la lógica de negocio.
3.  **Infrastructure**: Implementa el acceso a datos mediante **Entity Framework Core**, repositorios, la unidad de trabajo (`UnitOfWork`) y la configuración de PostgreSQL.
4.  **API**: El punto de entrada del sistema. Contiene los controladores, la configuración de **JWT**, Swagger y el pipeline de middleware.

## 🛠️ Tecnologías Utilizadas

- **.NET 8 SDK**
- **Entity Framework Core** (ORm)
- **PostgreSQL** (Base de datos)
- **Identity & JwtBearer** (Autenticación y Autorización)
- **Swagger/OpenAPI** (Documentación Interactiva)
- **Moq & xUnit** (Pruebas Unitarias)

## 🚀 Ejecución

### Dominio de Puertos
- **API**: `http://localhost:5000`
- **Swagger**: `http://localhost:5000/swagger`

### Con Docker (Recomendado)
Desde la raíz del repositorio, ejecuta:
```bash
docker compose up --build
```

### Localmente
Asegúrate de tener una instancia de PostgreSQL corriendo y configura la cadena de conexión en `appsettings.json`.
```bash
cd backend/assesment-employability
dotnet run --project src/AssessmentEmployability.API/AssessmentEmployability.API.csproj
```

## 🗄️ Migraciones de Base de Datos

Para manejar los cambios en el esquema de la base de datos, utilizamos Entity Framework Migrations.

**Crear una nueva migración:**
(Ejecutar desde `backend/assesment-employability`)
```bash
dotnet ef migrations add NombreDeLaMigracion \
  --project src/AssessmentEmployability.Infrastructure/AssessmentEmployability.Infrastructure.csproj \
  --startup-project src/AssessmentEmployability.API/AssessmentEmployability.API.csproj
```

**Actualizar la base de datos:**
```bash
dotnet ef database update \
  --project src/AssessmentEmployability.Infrastructure/AssessmentEmployability.Infrastructure.csproj \
  --startup-project src/AssessmentEmployability.API/AssessmentEmployability.API.csproj
```

## 🧪 Pruebas Unitarias

Para ejecutar los tests de lógica de negocio:
```bash
dotnet test tests/AssessmentEmployability.UnitTests/AssessmentEmployability.UnitTests.csproj
```

## Diagrama de clases
![Diagrama de clases](./wwwroot/imgs/image.png)