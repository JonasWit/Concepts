
# Use the official .NET 9.0 runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

# Use the .NET SDK for building the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the solution file and project files
COPY SM-Post/SM-Post.sln SM-Post/
COPY SM-Post/Post.Query/Post.Query.Api/Post.Query.Api.csproj SM-Post/Post.Query/Post.Query.Api/
COPY SM-Post/Post.Query/Post.Query.Domain/Post.Query.Domain.csproj SM-Post/Post.Query/Post.Query.Domain/
COPY SM-Post/Post.Query/Post.Query.Infrastructure/Post.Query.Infrastructure.csproj SM-Post/Post.Query/Post.Query.Infrastructure/
COPY SM-Post/Post.Common/Post.Common.csproj SM-Post/Post.Common/
COPY CQRS-ES/CQRS.Core/CQRS.Core.csproj CQRS-ES/CQRS.Core/

# Restore dependencies
WORKDIR /src/SM-Post
RUN dotnet restore Post.Query/Post.Query.Api/Post.Query.Api.csproj

# Copy the entire source code
WORKDIR /src
COPY SM-Post/ SM-Post/
COPY CQRS-ES/ CQRS-ES/

# Build the application
WORKDIR /src/SM-Post/Post.Query/Post.Query.Api
RUN dotnet build Post.Query.Api.csproj -c Release -o /app/build

# Publish the application
FROM build AS publish
WORKDIR /src/SM-Post/Post.Query/Post.Query.Api
RUN dotnet publish Post.Query.Api.csproj -c Release -o /app/publish

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create a directory for external configuration files
RUN mkdir -p /app/config

# Set default environment variables (can be overridden at runtime)
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80
ENV KAFKA_TOPIC=SocialMediaPostEvents

# Optional: Copy default appsettings (can be overridden by volume mount)
# The application will look for appsettings in the current directory by default
# Users can mount external appsettings.json to /app/config/appsettings.json

ENTRYPOINT ["dotnet", "Post.Query.Api.dll"]