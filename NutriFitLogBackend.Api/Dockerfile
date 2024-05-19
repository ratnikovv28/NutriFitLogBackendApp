FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NutriFitLogBackend.Api/NutriFitLogBackend.Api.csproj", "NutriFitLogBackend.Api/"]
COPY ["NutriFitLogBackend.Application/NutriFitLogBackend.Application.csproj", "NutriFitLogBackend.Application/"]
COPY ["NutriFitLogBackend.Domain/NutriFitLogBackend.Domain.csproj", "NutriFitLogBackend.Domain/"]
COPY ["NutriFitLogBackend.Infrastructure/NutriFitLogBackend.Infrastructure.csproj", "NutriFitLogBackend.Infrastructure/"]
RUN dotnet restore "NutriFitLogBackend.Api/NutriFitLogBackend.Api.csproj"
COPY . .
WORKDIR "/src/NutriFitLogBackend.Api"
RUN dotnet build "NutriFitLogBackend.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "NutriFitLogBackend.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NutriFitLogBackend.Api.dll"]
