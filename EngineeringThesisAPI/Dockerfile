# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /source
COPY . .
RUN dotnet restore "./EngineeringThesisAPI/EngineeringThesisAPI.csproj" --disable-parallel
RUN dotnet publish "./EngineeringThesisAPI/EngineeringThesisAPI.csproj" -c release -o /app --no-restore