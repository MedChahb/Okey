FROM mcr.microsoft.com/dotnet/sdk:8.0.101 AS build
WORKDIR /app


COPY server/OkeyServer/ ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime:8.0.1 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

EXPOSE 80
ENTRYPOINT ["dotnet", "OkeyServer.dll"]
