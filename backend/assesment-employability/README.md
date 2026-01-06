# Assessment Employability Backend

## Ejecución con Docker (Recomendado)

Para levantar el proyecto completo con su base de datos PostgreSQL, asegúrate de tener Docker instalado y ejecuta:

```bash
docker compose up --build
```

- El API estará disponible en `http://localhost:5000/swagger`
- La base de datos estará disponible en `localhost:5432`

## Migraciones Manuales

Si prefieres ejecutarlo localmente:

```bash
dotnet ef migrations add InitialCreate \
  --project ../AssessmentEmployability.Infrastructure/AssessmentEmployability.Infrastructure.csproj \
  --startup-project AssessmentEmployability.API.csproj
```

