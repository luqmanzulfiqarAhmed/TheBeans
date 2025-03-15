#!/bin/sh

# Wait for PostgreSQL to be ready
until pg_isready -h db -p 5432 -U admin; do
  echo "Waiting for PostgreSQL..."
  sleep 2
done

# Run EF Core migrations
dotnet ef database update --project ../TheBeans.Infrastructure

# Start the application
exec dotnet TheBeans.Api.dll