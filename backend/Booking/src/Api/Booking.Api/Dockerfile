FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Api/Booking.Api/Booking.Api.csproj", "Api/Booking.Api/"]
COPY ["Common/Booking.Common/Booking.Common.csproj", "Common/Booking.Common"]
COPY ["Modules/Booking.Modules.Users/Booking.Modules.Users.csproj" , "Modules/Booking.Modules.Users"]
COPY ["Modules/Booking.Modules.Mentorships/Booking.Modules.Mentorships.csproj" , "Modules/Booking.Modules.Mentorships"]
COPY ["Modules/Booking.Modules.Sessions/Booking.Modules.Sessions.csproj" , "Modules/Booking.Modules.Sessions"]


RUN dotnet restore "Booking.Api/Booking.Api.csproj"
COPY . .
WORKDIR "/src/Api/Booking.Api"
RUN dotnet build "./Booking.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Booking.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Booking.Api.dll"]
