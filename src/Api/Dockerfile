#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY Todo.Api/*.csproj Todo.Api/
COPY Todo.Application/*.csproj Todo.Application/
COPY Todo.Domain/*.csproj Todo.Domain/
COPY Todo.Persistence/*.csproj Todo.Persistence/
RUN dotnet restore Todo.Api/Todo.Api.csproj

# copy and build app and libraries
COPY Todo.Api/ Todo.Api/
COPY Todo.Application/ Todo.Application/
COPY Todo.Domain/ Todo.Domain/
COPY Todo.Persistence/ Todo.Persistence/
WORKDIR /src/Todo.Api
RUN dotnet build -c release --no-restore -o /app/build

FROM build AS publish
RUN dotnet publish "Todo.Api.csproj" -c Release --no-restore -o /app/publish

# final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Todo.Api.dll"]