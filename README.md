# Urna Electrónica - Página de Configuración

Sistema web de gestión y configuración para máquinas de votación electrónica (Urna Electrónica).

## Descripción

Esta es una aplicación web ASP.NET Core MVC que permite administrar las configuraciones de urnas electrónicas, incluyendo:

- **Gestión centralizada** de urnas electrónicas
- **Configuración** de parámetros de votación
- **Interfaz amigable** para administradores
- **Seguridad** en procesos electorales

## Requisitos

- **.NET 8.0** o superior
- **Navegador web** moderno (Chrome, Firefox, Safari, Edge)

## Estructura del Proyecto

```
├── Controllers/          # Controladores MVC
├── Models/              # Modelos de datos
├── Views/               # Vistas Razor
├── wwwroot/             # Activos estáticos (CSS, JavaScript)
├── Program.cs           # Configuración de la aplicación
├── appsettings.json     # Configuración
└── UrnaElectronica.csproj  # Archivo del proyecto
```

## Instalación y Ejecución

### 1. Restaurar dependencias

```bash
dotnet restore UrnaElectronica.csproj
```

### 2. Compilar el proyecto

```bash
dotnet build UrnaElectronica.csproj
```

### 3. Ejecutar la aplicación

```bash
dotnet run --project UrnaElectronica.csproj
```

La aplicación estará disponible en: **https://localhost:5001** o **http://localhost:5000**

## Características

### Página Principal
- Panel de bienvenida
- Acceso rápido a funcionalidades
- Información del sistema

### Gestión de Configuración
- **Listar** todas las urnas electrónicas
- **Crear** nueva configuración de urna
- **Ver** detalles de una urna
- **Editar** parámetros de configuración
- **Eliminar** urnas (con confirmación)

### Seguridad
- Validación de datos de entrada
- Protección contra acciones no autorizadas
- Interfaz de privacidad y seguridad

## Modelos de Datos

### ConfiguracionUrna
- ID
- Nombre de la urna
- Ubicación
- Número de mesa
- Departamento
- Municipio
- Estado (Activa/Inactiva)
- Fechas de creación y actualización

### Candidato
- ID
- Nombre
- Partido político
- Posición
- Descripción
- Estado activo
- Orden de presentación

### ProcesoElectoral
- ID
- Nombre del proceso
- Descripción
- Fechas de inicio y fin
- Estado (Pendiente, En Proceso, Completado)
- Lista de candidatos

## Tecnologías Utilizadas

- **Lenguaje**: C# 12
- **Framework**: ASP.NET Core 8.0
- **Arquitectura**: Modelo Vista Controlador (MVC)
- **Estilización**: CSS personalizado
- **JavaScript**: Vanilla JavaScript

## Desarrollo

### Estructura de carpetas

```
UrnaElectronica/
├── Controllers/
│   ├── HomeController.cs          # Controlador principal
│   └── ConfiguracionController.cs # Controlador de configuración
├── Models/
│   ├── ConfiguracionUrna.cs       # Modelo de configuración
│   ├── Candidato.cs               # Modelo de candidato
│   └── ProcesoElectoral.cs        # Modelo de proceso electoral
├── Views/
│   ├── Home/
│   │   ├── Index.cshtml           # Página principal
│   │   ├── Privacy.cshtml         # Política de privacidad
│   │   └── Error.cshtml           # Página de error
│   ├── Configuracion/
│   │   ├── Index.cshtml           # Listado de configuraciones
│   │   ├── Create.cshtml          # Crear nueva configuración
│   │   ├── Edit.cshtml            # Editar configuración
│   │   └── Delete.cshtml          # Confirmar eliminación
│   ├── Shared/
│   │   └── _Layout.cshtml         # Layout maestro
│   ├── _ViewImports.cshtml        # Importaciones globales
│   └── _ViewStart.cshtml          # Inicio de vistas
└── wwwroot/
    ├── css/
    │   └── style.css              # Estilos CSS principales
    └── js/
        └── main.js                # JavaScript principal
```

## Configuración

Editar `appsettings.json` para personalizar:

```json
{
  "UrnaElectronica": {
    "Titulo": "Configuración Urna Electrónica",
    "Descripcion": "Sistema de configuración para máquinas de votación electrónica",
    "Version": "1.0.0"
  }
}
```

## Scripts Útiles

### Limpiar build anterior
```bash
dotnet clean UrnaElectronica.csproj
```

### Reconstruir proyecto
```bash
dotnet clean UrnaElectronica.csproj && dotnet build UrnaElectronica.csproj
```

## Próximos Pasos

- [ ] Integrar base de datos (SQL Server/PostgreSQL)
- [ ] Agregar autenticación y autorización
- [ ] Implementar logging
- [ ] Crear API REST
- [ ] Agregar pruebas unitarias
- [ ] Configurar CI/CD
- [ ] Documentación de API

## Licencia

Este proyecto es parte del sistema de Urna Electrónica.

## Contacto

Para preguntas o sugerencias, contacte al equipo de desarrollo.

---

**Versión**: 1.0.0  
**Última actualización**: Febrero 9, 2026
