FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY . .

RUN dotnet restore "./GShark.gRPC/GShark.gRPC.csproj"

RUN dotnet build "./GShark.gRPC/GShark.gRPC.csproj" -c Release -o /out

FROM build AS publish
RUN dotnet publish "./GShark.gRPC/GShark.gRPC.csproj" -c Release -o /out --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /out .

ENTRYPOINT ["dotnet", "GShark.gRPC.dll"]