# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy NuGet config at root
COPY NuGet.config /src/NuGet.config

# Copy solution files
COPY ECommerce.ProductAPISolution/*.sln ECommerce.ProductAPISolution/

# Copy project files
COPY ECommerce.ProductAPISolution/ProductAPI.Presentation/*.csproj ECommerce.ProductAPISolution/ProductAPI.Presentation/
COPY ECommerce.ProductAPISolution/ProductAPI.Application/*.csproj ECommerce.ProductAPISolution/ProductAPI.Application/
COPY ECommerce.ProductAPISolution/ProductAPI.Domain/*.csproj ECommerce.ProductAPISolution/ProductAPI.Domain/
COPY ECommerce.ProductAPISolution/ProductAPI.Infrastructure/*.csproj ECommerce.ProductAPISolution/ProductAPI.Infrastructure/

# Shared Library
COPY ECommerce.SharedLibrarySolution/eCommerce.SharedLibrary/*.csproj ECommerce.SharedLibrarySolution/eCommerce.SharedLibrary/

# Restore (use absolute path)
RUN dotnet restore ECommerce.ProductAPISolution/ProductAPI.Presentation/ProductAPI.Presentation.csproj --configfile /src/NuGet.config

# Copy full source
COPY . .

# Publish (use absolute path again)
WORKDIR /src/ECommerce.ProductAPISolution/ProductAPI.Presentation
RUN dotnet publish ProductAPI.Presentation.csproj -c Release -o /app/publish --configfile /src/NuGet.config

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "ProductAPI.Presentation.dll"]