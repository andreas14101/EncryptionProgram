FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CryptographyConsoleApp/CryptographyConsoleApp.csproj", "CryptographyConsoleApp/"]
RUN dotnet restore "CryptographyConsoleApp/CryptographyConsoleApp.csproj"
COPY . .
WORKDIR "/src/CryptographyConsoleApp"
RUN dotnet build "CryptographyConsoleApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CryptographyConsoleApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CryptographyConsoleApp.dll"]
