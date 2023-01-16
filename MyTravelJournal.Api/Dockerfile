FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MyTravelJournal.Api/MyTravelJournal.Api.csproj", "MyTravelJournal.Api/"]
RUN dotnet restore "MyTravelJournal.Api/MyTravelJournal.Api.csproj"
COPY . .
WORKDIR "/src/MyTravelJournal.Api"
RUN dotnet build "MyTravelJournal.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyTravelJournal.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyTravelJournal.Api.dll"]
