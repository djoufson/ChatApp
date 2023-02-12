#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile 
#to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ChatApp.Api/ChatApp.Api.csproj", "ChatApp.Api/"]
COPY ["ChatApp.Shared/ChatApp.Shared.csproj", "ChatApp.Shared/"]
RUN dotnet restore "ChatApp.Api/ChatApp.Api.csproj"
COPY . .
WORKDIR "/src/ChatApp.Api"
RUN dotnet build "ChatApp.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChatApp.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChatApp.Api.dll"]