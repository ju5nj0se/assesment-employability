# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["src/AssessmentEmployability.API/AssessmentEmployability.API.csproj", "src/AssessmentEmployability.API/"]
COPY ["src/AssessmentEmployability.Application/AssessmentEmployability.Application.csproj", "src/AssessmentEmployability.Application/"]
COPY ["src/AssessmentEmployability.Domain/AssessmentEmployability.Domain.csproj", "src/AssessmentEmployability.Domain/"]
COPY ["src/AssessmentEmployability.Infrastructure/AssessmentEmployability.Infrastructure.csproj", "src/AssessmentEmployability.Infrastructure/"]

RUN dotnet restore "src/AssessmentEmployability.API/AssessmentEmployability.API.csproj"

# Copy the rest of the code
COPY . .

# Build and publish
RUN dotnet publish "src/AssessmentEmployability.API/AssessmentEmployability.API.csproj" -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "AssessmentEmployability.API.dll"]
