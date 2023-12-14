
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish WCA.Consumer.Api -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-jammy
WORKDIR /App
COPY --from=build-env /App/out .

RUN apt update

RUN apt install curl -y


ENV WCA_ports__http=8080
ENV WCA_ports__https=8081
EXPOSE 8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ./WCA.Consumer.Api 