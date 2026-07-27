FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY CommsManager.API/*.csproj CommsManager.API/
COPY CommsManager.Core/*.csproj CommsManager.Core/
COPY CommsManager.Infrastructure/*.csproj CommsManager.Infrastructure/

RUN dotnet restore CommsManager.API/CommsManager.API.csproj

COPY . .
WORKDIR /src/CommsManager.API
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "CommsManager.API.dll"]
