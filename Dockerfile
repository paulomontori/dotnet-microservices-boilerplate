# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY dotnet-microservices-boilerplate.csproj ./
RUN dotnet restore dotnet-microservices-boilerplate.csproj

# copy everything else and publish
COPY . .
RUN dotnet publish dotnet-microservices-boilerplate.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "dotnet-microservices-boilerplate.dll"]
