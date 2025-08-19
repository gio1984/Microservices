FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet build WebApi.Tests.csproj -c Release
ENTRYPOINT ["dotnet", "test", "WebApi.Tests.csproj", "--logger:trx"]