# Base image with ASP.NET runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build image with .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy only the project file first for caching
COPY ApplicationTracker/ApplicationTracker.csproj ./ApplicationTracker/
RUN dotnet restore ./ApplicationTracker/ApplicationTracker.csproj

# Copy the entire source code
COPY . .

# Build the project
WORKDIR /src/ApplicationTracker
RUN dotnet build "ApplicationTracker.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the project
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ApplicationTracker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Application entrypoint
ENTRYPOINT ["dotnet", "ApplicationTracker.dll"]
