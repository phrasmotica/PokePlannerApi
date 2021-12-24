#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["PokePlannerApi/PokePlannerApi.csproj", "PokePlannerApi/"]
COPY ["PokePlannerApi.Data/PokePlannerApi.Data.csproj", "PokePlannerApi.Data/"]
COPY ["PokePlannerApi.Clients/PokePlannerApi.Clients.csproj", "PokePlannerApi.Clients/"]
COPY ["PokePlannerApi.Models/PokePlannerApi.Models.csproj", "PokePlannerApi.Models/"]
RUN dotnet restore "PokePlannerApi/PokePlannerApi.csproj"
COPY . .
WORKDIR "/src/PokePlannerApi"
RUN dotnet build "PokePlannerApi.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "PokePlannerApi.csproj" -c Debug -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PokePlannerApi.dll"]