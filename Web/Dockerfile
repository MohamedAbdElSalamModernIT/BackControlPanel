FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY /Roi.sln ./
COPY ["Web/Web.csproj", "Web/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Common/Common.csproj", "Common/"]
COPY ["Infrastructuer/Infrastructuer.csproj", "Infrastructuer/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Persistence/Persistence.csproj", "Persistence/"]
COPY ["Persistence.MSSQL/Persistence.MSSQL.csproj", "Persistence.MSSQL/"]
RUN dotnet restore "Web/Web.csproj"
COPY . .
WORKDIR "/src/Web"
RUN dotnet build "Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN apt-get update
RUN apt-get install -y libfreetype6
RUN apt-get install -y libfontconfig1
RUN apt-get install -y vim

ENTRYPOINT ["dotnet", "Web.dll"]
