# Multi-stage Dockerfile for Booking (ASP.NET Core Razor Pages, .NET 8)

# For building .NET apps
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build


# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ARG APP_USER=app
ARG APP_UID=1000

# create a non-root user (id configurable) and working dir
RUN adduser --disabled-password --gecos "" --uid ${APP_UID} ${APP_USER} || true
WORKDIR /app

# make Kestrel listen on all interfaces at 8080
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
EXPOSE 8081

# Build / publish image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# copy csproj and restore (keeps layer caching) - expect build context is solution root
COPY ["Booking/Booking.csproj", "Booking/"]
RUN dotnet restore "Booking/Booking.csproj"

# copy remaining source and publish
COPY . .
WORKDIR /src/Booking
RUN dotnet publish "Booking.csproj" -c ${BUILD_CONFIGURATION} -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final

# copy published output and set ownership to the non-root user
COPY --chown=${APP_USER}:${APP_USER} --from=build /app/publish .

USER ${APP_USER}
ENTRYPOINT ["dotnet", "Booking.dll"]8