FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

COPY src/Slova.Backuper /app
WORKDIR /app
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime

COPY --from=build /app/out /app
WORKDIR /app

ENTRYPOINT ["dotnet", "Slova.Backuper.dll"]