FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001


FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS builder
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet build -c Release -o /app


FROM builder AS publish
RUN dotnet publish ./AnomalyDetection.Manager/AnomalyDetection.Manager.csproj -c Release -o /app


FROM base AS production
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["./AnomalyDetection.Manager"]