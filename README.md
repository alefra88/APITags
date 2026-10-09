# 🚗 API de TAGS - Sistema Integral de Telepeaje

Esta es una API RESTful desarrollada en **ASP.NET Core** diseñada para gestionar el ciclo de vida de los TAGs de telepeaje. El sistema utiliza **AWS DynamoDB** como motor de persistencia NoSQL y está estructurado bajo los principios de **Arquitectura Multicapa (N-Tier)** para garantizar la seguridad, escalabilidad y separación de responsabilidades.

## 🏗️ Arquitectura del Proyecto

El código está dividido en capas estrictas para evitar el acoplamiento y proteger el dominio:

*   **Models (Dominio):** Representación de la entidad central `Tag` con su Partition Key (`NumeroTag`).
*   **DTOs:** Contratos de entrada (`TagCreateRequestDTO`) que incluyen validaciones con Expresiones Regulares para sanitizar datos (ej. formato de placas).
*   **Repositories (Acceso a Datos):** Integración directa con AWS SDK para ejecutar operaciones sobre DynamoDB, aislando la lógica de base de datos del resto del sistema.
*   **Services (Lógica de Negocio):** Centraliza la creación segura de identificadores, asignación de saldos iniciales y estados, evitando manipulaciones desde el cliente.
*   **Controllers (Presentación):** Exponen los endpoints HTTP, evalúan validaciones y delegan el procesamiento.

## 🛠️ Tecnologías Utilizadas

*   C# / .NET (ASP.NET Core Web API)
*   AWS SDK for .NET (Amazon.DynamoDBv2)
*   Swashbuckle.AspNetCore (Documentación con Swagger)

## 📦 Instalación de Dependencias

Para ejecutar este proyecto, necesitas instalar los paquetes NuGet correspondientes a las integraciones de AWS y Swagger. Abre tu terminal en la carpeta raíz del proyecto y ejecuta:

```bash
# Instalar el SDK de AWS para DynamoDB
dotnet add package AWSSDK.DynamoDBv2

# Instalar Swashbuckle para habilitar la interfaz gráfica de Swagger
dotnet add package Swashbuckle.AspNetCore