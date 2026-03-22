# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy NuGet config at root
COPY NuGet.config /src/NuGet.config

# Copy solution files
COPY ECommerce.OrderAPISolution/*.sln ECommerce.OrderAPISolution/

# Copy project files
COPY ECommerce.OrderAPISolution/OrderAPI.Presentation/*.csproj ECommerce.OrderAPISolution/OrderAPI.Presentation/
COPY ECommerce.OrderAPISolution/OrderAPI.Application/*.csproj ECommerce.OrderAPISolution/OrderAPI.Application/
COPY ECommerce.OrderAPISolution/OrderAPI.Domain/*.csproj ECommerce.OrderAPISolution/OrderAPI.Domain/
COPY ECommerce.OrderAPISolution/OrderAPI.Infrastructure/*.csproj ECommerce.OrderAPISolution/OrderAPI.Infrastructure/

# Shared Library
COPY ECommerce.SharedLibrarySolution/eCommerce.SharedLibrary/*.csproj ECommerce.SharedLibrarySolution/eCommerce.SharedLibrary/

# Restore (use absolute path)
RUN dotnet restore ECommerce.OrderAPISolution/OrderAPI.Presentation/OrderAPI.Presentation.csproj --configfile /src/NuGet.config

# Copy full source
COPY . .

# Publish (use absolute path again)
WORKDIR /src/ECommerce.OrderAPISolution/OrderAPI.Presentation
RUN dotnet publish OrderAPI.Presentation.csproj -c Release -o /app/publish --configfile /src/NuGet.config

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "OrderAPI.Presentation.dll"]