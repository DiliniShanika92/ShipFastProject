# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /source
COPY . .
RUN dotnet restore "./ShipFast.Authentication.csproj" --disable-parallel
RUN dotnet publish "./ShipFast.Authentication.csproj" -c release -o /app --no-restore

# Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal
WORKDIR /app
COPY --from=build /app ./

EXPOSE 3000

ENTRYPOINT ["dotnet", "ShipFast.Authentication.dll"]