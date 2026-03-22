# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy NuGet config at root
COPY NuGet.config /src/NuGet.config

# Copy solution files
COPY ECommerce.AuthenticationAPISolution/*.sln ECommerce.AuthenticationAPISolution/

# Copy project files
COPY ECommerce.AuthenticationAPISolution/AuthenticationAPI.Presentation/*.csproj ECommerce.AuthenticationAPISolution/AuthenticationAPI.Presentation/
COPY ECommerce.AuthenticationAPISolution/AuthenticationAPI.Application/*.csproj ECommerce.AuthenticationAPISolution/AuthenticationAPI.Application/
COPY ECommerce.AuthenticationAPISolution/AuthenticationAPI.Domain/*.csproj ECommerce.AuthenticationAPISolution/AuthenticationAPI.Domain/
COPY ECommerce.AuthenticationAPISolution/AuthenticationAPI.Infrastructure/*.csproj ECommerce.AuthenticationAPISolution/AuthenticationAPI.Infrastructure/

# Shared Library
COPY ECommerce.SharedLibrarySolution/eCommerce.SharedLibrary/*.csproj ECommerce.SharedLibrarySolution/eCommerce.SharedLibrary/

# Restore (use absolute path)
RUN dotnet restore ECommerce.AuthenticationAPISolution/AuthenticationAPI.Presentation/AuthenticationAPI.Presentation.csproj --configfile /src/NuGet.config

# Copy full source
COPY . .

# Publish (use absolute path again)
WORKDIR /src/ECommerce.AuthenticationAPISolution/AuthenticationAPI.Presentation
RUN dotnet publish AuthenticationAPI.Presentation.csproj -c Release -o /app/publish --configfile /src/NuGet.config

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "AuthenticationAPI.Presentation.dll"]