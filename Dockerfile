FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ParkingLot.sln .
COPY ParkingLot.Api/ParkingLot.Api.csproj ./ParkingLot.Api/
COPY ParkingLot.Data/ParkingLot.Data.csproj ./ParkingLot.Data/
COPY ParkingLot.Tickets/ParkingLot.Tickets.csproj ./ParkingLot.Tickets/
COPY ParkingLot.Tickets.Tests/ParkingLot.Tickets.Tests.csproj ./ParkingLot.Tickets.Tests/
RUN dotnet restore ParkingLot.sln

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o dist

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0
WORKDIR /app
COPY --from=build-env /app/dist .
ENTRYPOINT ["dotnet", "ParkingLot.Api.dll"]
