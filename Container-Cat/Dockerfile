FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /Container-Cat

# Copy everything
COPY *.csproj .
# Restore as distinct layers
RUN dotnet restore --use-current-runtime  
# Build and publish a release
COPY . .
RUN dotnet publish -c Debug -o /app

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app .
ENTRYPOINT ["dotnet", "Container-Cat.dll"]
