FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["WuQu.TestPublisher/WuQu.TestPublisher.csproj", "WuQu.TestPublisher/"]
RUN dotnet restore "WuQu.TestPublisher/WuQu.TestPublisher.csproj"
COPY . .
WORKDIR "/src/WuQu.TestPublisher"
RUN dotnet build "WuQu.TestPublisher.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WuQu.TestPublisher.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WuQu.TestPublisher.dll"]
