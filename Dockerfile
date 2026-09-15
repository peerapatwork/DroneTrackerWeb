# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY ["DroneTrackerWeb.csproj", "./"]
RUN dotnet restore "DroneTrackerWeb.csproj"

# Copy remaining source code and publish
COPY . .
RUN dotnet publish "DroneTrackerWeb.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "DroneTrackerWeb.dll"]
