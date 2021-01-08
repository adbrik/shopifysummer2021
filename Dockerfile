FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /imagerepo

# Copy everything else and build
COPY . .
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /imagerepo
COPY --from=build /imagerepo/out .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Shopify2021Summer.dll"]