# 🚗 API de TAGS - Sistema Integral de Telepeaje

Esta es una API RESTful desarrollada en **ASP.NET Core (.NET 10)** diseñada para gestionar el ciclo de vida de los TAGs de telepeaje y la autenticación de usuarios. El sistema utiliza **AWS DynamoDB** como motor de persistencia NoSQL, está optimizado para ejecutarse en entornos serverless mediante **AWS Lambda** y está estructurado bajo los principios de **Arquitectura Multicapa (N-Tier)**.

## 🏗️ Arquitectura del Proyecto

El código está dividido en capas estrictas para evitar el acoplamiento y proteger el dominio:

- **Models (Dominio):** Representación de entidades centrales (`Tag`, `Usuario`) con sus respectivas claves de partición.
- **DTOs:** Contratos de entrada y salida (`LoginRequestDTO`, `TagCreateRequestDTO`) que incluyen validaciones con Expresiones Regulares para sanitizar datos (ej. formato de placas).
- **Repositories (Acceso a Datos):** Integración directa con AWS SDK para DynamoDB (`Amazon.DynamoDBv2.DataModel.DynamoDBContext`), aislando la lógica de base de datos.
- **Services (Lógica de Negocio):** Centraliza la autenticación, generación de tokens JWT y la lógica operativa.
- **Controllers (Presentación):** Exponen los endpoints HTTP seguros (`/api/auth`, `/api/tags`).

## 🛠️ Tecnologías y Servicios AWS Utilizados

- C# / .NET 10 (ASP.NET Core Web API)
- AWS Lambda & Lambda Function URLs (Alojamiento serverless del backend)
- AWS SDK for .NET (`AWSSDK.DynamoDBv2`)
- Amazon DynamoDB (Base de datos NoSQL para usuarios y tags)
- JSON Web Tokens (JWT) para autenticación basada en claims
- Herramienta global de despliegue (`Amazon.Lambda.Tools`)

## 📦 Instalación de Dependencias

Para ejecutar este proyecto, necesitas instalar los paquetes NuGet correspondientes a las integraciones de AWS, JWT y utilidades. Abre tu terminal en la carpeta raíz del proyecto y ejecuta:

```bash
# Instalar el SDK de AWS para DynamoDB y el adaptador para ASP.NET Core en Lambda
dotnet add package AWSSDK.DynamoDBv2
dotnet add package Amazon.Lambda.AspNetCoreServer

# Instalar autenticación JWT para ASP.NET Core
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer

# Herramienta global de AWS Lambda para CLI
dotnet tool install -g Amazon.Lambda.Tools
```

## ⚙️ Configuración y Ejecución Local

### 1. Archivo de configuración (`appsettings.json`)
Asegúrate de contar con una estructura local que incluya la configuración para JWT:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "Jwt": {
    "Key": "ClaveSecreta",
    "Issuer": "API",
    "Audience": "UsuariosAngular"
  }
}
```

### 2. Ejecución local
```bash
dotnet restore
dotnet run
```
*La API correrá por defecto en `http://localhost:5045`.*

## ☁️ Despliegue en Producción (AWS Lambda)

### 1. Empaquetado para Lambda
Ejecuta el siguiente comando para compilar y empaquetar tu API en un archivo `.zip` optimizado para .NET 10:

```bash
dotnet lambda package -c Release -f net10.0 -o bin/net10.0/api-telepeaje.zip
```

### 2. Configuración en la Consola de AWS
1. **Función Lambda:** Sube el archivo `api-telepeaje.zip` a tu función Lambda.
2. **Variables de Entorno (Configuration > Environment variables):** Define las credenciales del JWT utilizando doble guion bajo (`__`) para la jerarquía de .NET en Linux:
   - `Jwt__Key` = `SuperSecretaClaveDeTelepeaje2026!!!`
   - `Jwt__Issuer` = `AppTelepeaje`
   - `Jwt__Audience` = `UsuariosAngular`
3. **Permisos de IAM:** Asocia una política a tu rol de ejecución de Lambda que permita realizar operaciones sobre DynamoDB (ej. `AmazonDynamoDBFullAccess` o permisos específicos de lectura/escritura sobre tus tablas `UsuariosTelepeaje` y `Tags`).
4. **CORS:** Configura la *Function URL* de tu Lambda para permitir de manera exclusiva el origen de tu frontend en S3 y las cabeceras `content-type` y `authorization` (omitiendo la configuración de CORS en `Program.cs` para evitar conflictos de duplicidad en las cabeceras del navegador).