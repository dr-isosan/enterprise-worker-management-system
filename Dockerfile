# Use the official .NET 8.0 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore dependencies
COPY WorkerApp/*.csproj ./WorkerApp/
RUN dotnet restore WorkerApp/

# Copy everything else and build
COPY . .
WORKDIR /app/WorkerApp
RUN dotnet publish -c Release -o out

# Use the runtime image for final stage
FROM mcr.microsoft.com/dotnet/runtime:8.0-windowsservercore-ltsc2022
WORKDIR /app

# Install SQL Server Express (for self-contained option)
# Note: For production, use external SQL Server instance

COPY --from=build /app/WorkerApp/out .

# Expose the application port (if needed for future web interface)
EXPOSE 80

ENTRYPOINT ["dotnet", "WorkerApp.dll"]
