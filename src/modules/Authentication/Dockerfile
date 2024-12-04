FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["eShop.Auth/eShop.Auth.csproj", "eShop.Auth/"]
COPY ["eShop.Auth.Infrastructure/eShop.Auth.Infrastructure.csproj", "eShop.Auth.Infrastructure/"]
COPY ["eShop.Auth/eShop.Auth.csproj", "eShop.Auth/"]
COPY ["eShop.Auth.Application/eShop.Auth.Application.csproj","eShop.Auth.Application/"]
COPY ["eShop.Auth.Domain/eShop.Auth.Domain.csproj","eShop.Auth.Domain/"]
RUN dotnet restore "eShop.Auth/eShop.Auth.csproj"
COPY . .
WORKDIR "/src/eShop.Auth"
RUN dotnet build "eShop.Auth.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "eShop.Auth.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "eShop.Auth.dll"]
