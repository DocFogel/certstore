# Learn about building .NET container images:
# https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md --platform=$BUILDPLATFORM
FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build
ARG TARGETARCH
WORKDIR /certstore

# copy csproj and restore as distinct layers -a $TARGETARCH
COPY certstore/*.csproj .
RUN dotnet restore

# copy and publish app and libraries -a $TARGETARCH
COPY certstore .
RUN dotnet publish --no-restore -o /app


# final stage/image
FROM mcr.microsoft.com/dotnet/runtime:8.0-jammy-chiseled
WORKDIR /app
COPY --from=build /app .
CMD ["./certstore"]