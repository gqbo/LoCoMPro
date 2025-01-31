# Usar la imagen base de .NET SDK para construir la aplicación
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copiar el archivo .csproj y restaurar las dependencias
COPY ./LoCoMPro_LV.csproj ./
RUN dotnet restore

# Copiar el resto de los archivos del proyecto
COPY . .

# Publicar el proyecto específico usando el archivo .csproj
RUN dotnet publish ./LoCoMPro_LV.csproj -c Release -o /app/PublishedApp --no-self-contained

# Imagen base para el runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /webapp

# Copiar los archivos publicados desde la imagen de construcción
COPY --from=build /app/PublishedApp .

# Exponer los puertos necesarios
EXPOSE 8080

# Comando para iniciar la aplicación
ENTRYPOINT ["dotnet", "LoCoMPro_LV.dll"]