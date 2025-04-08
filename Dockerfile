# create the build instance
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build

WORKDIR /src                                                                    
COPY ./src ./

# restore solution
RUN dotnet restore One.Settix.Cli.sln

WORKDIR /src/One.Settix.Cli

# build and publish project   
RUN dotnet build One.Settix.Cli.csproj -c Release -o /app                                         
RUN dotnet publish One.Settix.Cli.csproj -c Release -o /app/published

# create the runtime instance 
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS runtime 

# add globalization support
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

WORKDIR /app
                                                            
COPY --from=build /app/published .                         
                            
ENTRYPOINT ["dotnet", "One.Settix.Cli.dll"]
