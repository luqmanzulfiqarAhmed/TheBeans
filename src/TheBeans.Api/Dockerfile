# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["TheBeans.sln", "."]
COPY ["src/TheBeans.Core/*.csproj", "src/TheBeans.Core/"]
COPY ["src/TheBeans.Application/*.csproj", "src/TheBeans.Application/"]
COPY ["src/TheBeans.Infrastructure/*.csproj", "src/TheBeans.Infrastructure/"]
COPY ["src/TheBeans.Api/*.csproj", "src/TheBeans.Api/"]

# Restore dependencies
RUN dotnet restore "TheBeans.sln"

# Copy all source code
COPY . .

# Build the API
WORKDIR "/src/src/TheBeans.Api"
RUN dotnet build -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=publish /app/publish .

# Entrypoint script to run migrations and start the app
COPY ["entrypoint.sh", "./"]
RUN chmod +x entrypoint.sh

ENTRYPOINT ["./entrypoint.sh"]