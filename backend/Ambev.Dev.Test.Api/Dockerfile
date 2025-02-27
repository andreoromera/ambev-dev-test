# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Ambev.Dev.Test.Api/Ambev.Dev.Test.Api.csproj", "Ambev.Dev.Test.Api/"]
COPY ["Ambev.Dev.Test.Application/Ambev.Dev.Test.Application.csproj", "Ambev.Dev.Test.Application/"]
COPY ["Ambev.Dev.Test.Data/Ambev.Dev.Test.Data.csproj", "Ambev.Dev.Test.Data/"]
COPY ["Ambev.Dev.Test.Domain/Ambev.Dev.Test.Domain.csproj", "Ambev.Dev.Test.Domain/"]
COPY ["Ambev.Dev.Test.IoC/Ambev.Dev.Test.IoC.csproj", "Ambev.Dev.Test.IoC/"]
RUN dotnet restore "./Ambev.Dev.Test.Api/Ambev.Dev.Test.Api.csproj"
COPY . .
WORKDIR "/src/Ambev.Dev.Test.Api"
RUN dotnet build "./Ambev.Dev.Test.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Ambev.Dev.Test.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ambev.Dev.Test.Api.dll"]