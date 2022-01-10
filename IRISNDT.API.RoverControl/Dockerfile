#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["IRISNDT.API.RoverControl/IRISNDT.API.RoverControl.csproj", "IRISNDT.API.RoverControl/"]
RUN dotnet restore "IRISNDT.API.RoverControl/IRISNDT.API.RoverControl.csproj"
COPY . .
WORKDIR "/src/IRISNDT.API.RoverControl"
RUN dotnet build "IRISNDT.API.RoverControl.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IRISNDT.API.RoverControl.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IRISNDT.API.RoverControl.dll"]