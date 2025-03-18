#!/bin/sh

# Wait for the database to be ready
echo "Waiting for PostgreSQL to start..."
while ! nc -z db 5432; do
  sleep 1
done
echo "PostgreSQL started."

# Start the application
echo "Starting the application..."
dotnet TheBeans.Api.dll
