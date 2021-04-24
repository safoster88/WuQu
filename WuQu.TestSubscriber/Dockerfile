FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["WuQu.TestClient/WuQu.TestClient.csproj", "WuQu.TestClient/"]
RUN dotnet restore "WuQu.TestClient/WuQu.TestClient.csproj"
COPY . .
WORKDIR "/src/WuQu.TestClient"
RUN dotnet build "WuQu.TestClient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WuQu.TestClient.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WuQu.TestClient.dll"]
