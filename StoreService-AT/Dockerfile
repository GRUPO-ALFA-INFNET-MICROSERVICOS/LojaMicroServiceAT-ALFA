FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 3064

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["StoreService-AT/StoreService-AT.csproj", "StoreService-AT/"]
RUN dotnet restore "StoreService-AT/StoreService-AT.csproj"
COPY . .
WORKDIR /src/StoreService-AT
RUN dotnet build "StoreService-AT.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StoreService-AT.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "StoreService-AT.dll"]
CMD ASPNETCORE_URLS=http://*:3064 dotnet StoreService-AT.dll