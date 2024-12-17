FROM mcr.microsoft.com/dotnet/aspnet-sdk:8.0 AS base
WORKDIR /app

COPY .././ .

# Restore dependencies and build the application
RUN dotnet restore
RUN dotnet build

# Start the application with dotnet watch to enable real-time updates
CMD ["dotnet", "watch", "run"]