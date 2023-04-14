#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Permission.Api/Permissions.Api.csproj", "src/Permission.Api/"]
COPY ["src/Permission.Domain/Permissions.Domain.csproj", "src/Permission.Domain/"]
COPY ["src/Permission.Infrastructure/Permissions.Infrastructure.csproj", "src/Permission.Infrastructure/"]
RUN dotnet restore "src/Permission.Api/Permissions.Api.csproj"
COPY . .
WORKDIR "/src/src/Permission.Api"
RUN dotnet build "Permissions.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Permissions.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Permissions.Api.dll"]