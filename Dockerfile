FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy trực tiếp vì file .csproj nằm ngay thư mục gốc
COPY ["ECommerceMVC.csproj", "./"]
RUN dotnet restore "ECommerceMVC.csproj"

# Copy toàn bộ code và build
COPY . .
RUN dotnet build "ECommerceMVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ECommerceMVC.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Chắc chắn rằng tên file xuất ra là ECommerceMVC.dll
ENTRYPOINT ["dotnet", "ECommerceMVC.dll"]