# Dockerfile for building the application
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
RUN apt-get update && apt-get install -y locales

WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore "IBSYS.PPS.csproj"

# copy everything else and build app
COPY . ./
RUN dotnet publish "IBSYS.PPS.csproj" -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "IBSYS.PPS.dll"]