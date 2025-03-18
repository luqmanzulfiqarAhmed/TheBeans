# Use the official .NET SDK image for the build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set up the working directory
WORKDIR /src

# Copy the solution and project files
COPY TheBeans.sln ./
COPY src/TheBeans.Core/*.csproj src/TheBeans.Core/
COPY src/TheBeans.Application/*.csproj src/TheBeans.Application/
COPY src/TheBeans.Infrastructure/*.csproj src/TheBeans.Infrastructure/
COPY src/TheBeans.Api/*.csproj src/TheBeans.Api/
COPY src/TheBeans.Tests/*.csproj src/TheBeans.Tests/ 

# Restore dependencies
RUN dotnet restore "TheBeans.sln"

# Copy the entire code
COPY . .

# Install dotnet-ef tool
RUN dotnet tool install --global dotnet-ef

# Build the application
RUN dotnet build "TheBeans.sln" -c Release -o /app/build

# Publish the application
RUN dotnet publish "TheBeans.sln" -c Release -o /app/publish

# Use the official .NET runtime image for the final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

# Set the working directory
WORKDIR /app

# Ensure the application listens on port 80
ENV ASPNETCORE_URLS=http://+:80

# Copy the published files from the build stage
COPY --from=build /app/publish .

# Install netcat for the entrypoint script and clean up apt-get lists
RUN apt-get update && apt-get install -y netcat-openbsd && rm -rf /var/lib/apt/lists/*

# Copy the dotnet-ef tool from the build stage
COPY --from=build /root/.dotnet/tools /root/.dotnet/tools

# Add dotnet tools to PATH
ENV PATH="$PATH:/root/.dotnet/tools"

# Copy the entrypoint script and make it executable
COPY entrypoint.sh ./
RUN chmod +x entrypoint.sh

# Start the application
ENTRYPOINT ["./entrypoint.sh"]
