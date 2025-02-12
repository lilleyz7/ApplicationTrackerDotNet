# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Use the .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy the project files and restore dependencies
COPY *.sln ./
COPY ApplicationTracker/*.csproj ./ApplicationTracker/
RUN dotnet restore ApplicationTracker/*.csproj

# Copy the entire project and build
COPY . .
RUN dotnet publish -c Release -o /out

# Use a smaller runtime image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /out ./
ENTRYPOINT ["dotnet", "ApplicationTracker.dll"]