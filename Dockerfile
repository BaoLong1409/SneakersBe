FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Sneakers/Sneakers.csproj", "Sneakers/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["DataAccess/DataAccess.csproj", "DataAccess/"]

RUN dotnet restore "Sneakers/Sneakers.csproj"

COPY . .

WORKDIR /src/Sneakers
#Build project ở chế độ Release, xuất kết quả (dll + config) vào thư mục /app/publish
RUN dotnet publish "Sneakers.csproj" -c Release -o /app/publish 

#Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
COPY wait-for-it.sh /wait-for-it.sh
#Cấp quyền thực thi cho file script
RUN chmod +x /wait-for-it.sh
ENTRYPOINT ["/wait-for-it.sh", "db:1433", "--timeout=60", "--","dotnet", "Sneakers.dll"]