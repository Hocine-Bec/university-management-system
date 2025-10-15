# ============================================
# STAGE 1: BUILD
# ============================================
# Use the official .NET SDK image (has all build tools)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set working directory inside the container
WORKDIR /src

# Copy solution file
COPY Presentation/Presentation.sln ./

# Copy all project files (we need these for restore)
COPY Domain/Domain.csproj ./Domain/
COPY Applications/Applications.csproj ./Applications/
COPY Infrastructure/Infrastructure.csproj ./Infrastructure/
COPY Presentation/Presentation.csproj ./Presentation/

# Restore NuGet packages (downloads dependencies)
RUN dotnet restore "Presentation/Presentation.csproj"

# Copy the rest of the source code
COPY Domain/ ./Domain/
COPY Applications/ ./Applications/
COPY Infrastructure/ ./Infrastructure/
COPY Presentation/ ./Presentation/

# Build the application
WORKDIR /src/Presentation
RUN dotnet build "Presentation.csproj" -c Release -o /app/build

# Publish the application (creates optimized runtime files)
RUN dotnet publish "Presentation.csproj" -c Release -o /app/publish

# ============================================
# STAGE 2: RUNTIME
# ============================================
# Use the smaller runtime-only image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

# Set working directory
WORKDIR /app

# Copy the published app from the build stage
COPY --from=build /app/publish .

# Copy Docker-specific .env file
COPY .env.docker ./.env

# Expose port 8080 (or whatever port your app uses)
EXPOSE 8080

# Set environment variable for ASP.NET Core
ENV ASPNETCORE_URLS=http://+:8080

# Run the application
ENTRYPOINT ["dotnet", "Presentation.dll"]
