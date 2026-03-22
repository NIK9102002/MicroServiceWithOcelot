# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy NuGet config at root
COPY NuGet.config /src/NuGet.config

# Copy solution files
COPY ECommerce.APIGatewaySolution/*.sln ECommerce.APIGatewaySolution/

# Copy project files
COPY ECommerce.APIGatewaySolution/APIGateway.Presentation/*.csproj ECommerce.APIGatewaySolution/APIGateway.Presentation/
# Shared Library
COPY ECommerce.SharedLibrarySolution/eCommerce.SharedLibrary/*.csproj ECommerce.SharedLibrarySolution/eCommerce.SharedLibrary/

# Restore (use absolute path)
RUN dotnet restore ECommerce.APIGatewaySolution/APIGateway.Presentation/APIGateway.Presentation.csproj --configfile /src/NuGet.config

# Copy full source
COPY . .

# Publish (use absolute path again)
WORKDIR /src/ECommerce.APIGatewaySolution/APIGateway.Presentation
RUN dotnet publish APIGateway.Presentation.csproj -c Release -o /app/publish --configfile /src/NuGet.config

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "APIGateway.Presentation.dll"]