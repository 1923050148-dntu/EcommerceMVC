FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Sửa lại đường dẫn copy để Render tìm thấy file dự án
COPY ["ECommerceMVC/ECommerceMVC.csproj", "ECommerceMVC/"]
RUN dotnet restore "ECommerceMVC/ECommerceMVC.csproj"

COPY . .
WORKDIR "/src/ECommerceMVC"
RUN dotnet build "ECommerceMVC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ECommerceMVC.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ECommerceMVC.dll"]