FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ModelX/ModelX.csproj", "ModelX/"]
RUN dotnet restore "ModelX/ModelX.csproj"
COPY . .
WORKDIR "/src/ModelX"
RUN dotnet build "ModelX.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ModelX.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ModelX.dll"]
