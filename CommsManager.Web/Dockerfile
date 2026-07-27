FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY CommsManager.Web/*.csproj CommsManager.Web/
COPY CommsManager.Shared/*.csproj CommsManager.Shared/

RUN dotnet restore CommsManager.Web/CommsManager.Web.csproj

COPY . .
WORKDIR /src/CommsManager.Web
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "CommsManager.Web.dll"]
