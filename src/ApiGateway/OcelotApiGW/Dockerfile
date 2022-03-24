#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ApiGateway/OcelotApiGW/OcelotApiGW.csproj", "ApiGateway/OcelotApiGW/"]
RUN dotnet restore "ApiGateway/OcelotApiGW/OcelotApiGW.csproj"
COPY . .
WORKDIR "/src/ApiGateway/OcelotApiGW"
RUN dotnet build "OcelotApiGW.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OcelotApiGW.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OcelotApiGW.dll"]
