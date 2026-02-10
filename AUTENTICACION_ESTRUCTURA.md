# Guía de Estructura de Autenticación

## 📋 Descripción General

La aplicación utiliza una arquitectura **Service-Repository Pattern** para manejar la autenticación y gestión de usuarios. Esto permite separar la lógica de negocios de la lógica de acceso a datos, facilitando la conexión futura con una base de datos real.

## 🏗️ Estructura Actual

### Componentes Principales

1. **IAuthService** (`Services/IAuthService.cs`)
   - Interfaz que define los métodos de autenticación
   - Ya implementada en `AuthService.cs`

2. **IUsuarioRepository** (`Data/IUsuarioRepository.cs`)
   - Interfaz que define las operaciones CRUD de usuarios
   - Implementación simulada: `UsuarioRepositorySimulado.cs`

3. **AuthController** (`Controllers/AuthController.cs`)
   - Maneja LOGIN, LOGOUT y validación de sesiones
   - Usa inyección de dependencias de `IAuthService`

4. **BaseController** (`Controllers/BaseController.cs`)
   - Proporciona métodos comunes para todos los controladores
   - Verifica autenticación automáticamente
   - Proporciona datos de sesión a las vistas

5. **Middleware de Autenticación** (`Middleware/AutenticacionMiddleware.cs`)
   - Protege rutas que requieren autenticación
   - Redirige a login si no hay sesión activa

## 🔄 Cómo Funciona la Sesión

### Login
1. Usuario ingresa email y contraseña
2. `AuthController.Login()` llama a `IAuthService.ValidarCredencialesAsync()`
3. El servicio consulta `IUsuarioRepository` para obtener el usuario
4. Si las credenciales son válidas, se guarda en `HttpContext.Session`:
   - `UsuarioEmail`
   - `UsuarioNombre`
   - `UsuarioRol`
   - `UsuarioId`

### Verificación de Sesión
- El middleware intercepta las rutas y verifica que exista `UsuarioEmail` en sesión
- Si no existe, redirige a `/Auth/Login`

### Logout
- `AuthController.Logout()` limpia toda la sesión
- `HttpContext.Session.Clear()`

## 🔌 Cómo Conectar una Base de Datos Real

### Paso 1: Instalar Entity Framework
```bash
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
```

### Paso 2: Crear DbContext
Crear archivo `Data/UrnaDbContext.cs`:

```csharp
using Microsoft.EntityFrameworkCore;
using UrnaElectronica.Models;

namespace UrnaElectronica.Data
{
    public class UrnaDbContext : DbContext
    {
        public UrnaDbContext(DbContextOptions<UrnaDbContext> options) : base(options) { }
        
        public DbSet<Usuario> Usuarios { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configuraciones específicas de la tabla
            modelBuilder.Entity<Usuario>().HasKey(u => u.Id);
            modelBuilder.Entity<Usuario>().HasIndex(u => u.Email).IsUnique();
        }
    }
}
```

### Paso 3: Crear Implementación con Entity Framework
Crear archivo `Data/UsuarioRepositoryEF.cs`:

```csharp
using Microsoft.EntityFrameworkCore;
using UrnaElectronica.Models;

namespace UrnaElectronica.Data
{
    public class UsuarioRepositoryEF : IUsuarioRepository
    {
        private readonly UrnaDbContext _context;
        
        public UsuarioRepositoryEF(UrnaDbContext context)
        {
            _context = context;
        }
        
        public async Task<List<Usuario>> ObtenerTodosAsync()
        {
            return await _context.Usuarios.Where(u => u.Activo).ToListAsync();
        }
        
        public async Task<Usuario> ObtenerPorIdAsync(int id)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id && u.Activo);
        }
        
        // ... Implementar resto de métodos
    }
}
```

### Paso 4: Actualizar Program.cs

Cambiar en `Program.cs`:

```csharp
// De:
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepositorySimulado>();

// A:
builder.Services.AddDbContext<UrnaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepositoryEF>();
```

### Paso 5: Configurar Connection String
En `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=TuServidor;Database=UrnaElectronica;User Id=sa;Password=TuPassword;"
  }
}
```

### Paso 6: Crear Migraciones
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## 📝 Modelo Usuario

```csharp
public class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Email { get; set; }
    public string Contraseña { get; set; }  // Hash SHA256
    public string Rol { get; set; }         // Admin, Operador, Supervisor
    public bool Activo { get; set; }
}
```

## 🛡️ Seguridad

- ✅ Contraseñas hasheadas con SHA256
- ✅ Sesiones HTTP-Only
- ✅ Validación CSRF en forms
- ✅ Anti-forgery tokens
- ✅ Redireccionamiento automático a login

## 🧪 Usuarios de Prueba Actuales

| Email | Contraseña | Rol |
|-------|-----------|-----|
| admin@urna.gov | admin123 | Admin |
| operador@urna.gov | operador123 | Operador |
| supervisor@urna.gov | supervisor123 | Supervisor |

## 📚 Archivos Relevantes

```
├── Services/
│   ├── IAuthService.cs
│   └── AuthService.cs
├── Data/
│   ├── IUsuarioRepository.cs
│   └── UsuarioRepositorySimulado.cs  ← Reemplazar con UsuarioRepositoryEF
├── Controllers/
│   ├── AuthController.cs
│   └── BaseController.cs
├── Middleware/
│   └── AutenticacionMiddleware.cs
├── Models/
│   └── Usuario.cs
└── Program.cs
```

## 🚀 Próximos Pasos

1. Crear la base de datos SQL Server/PostgreSQL
2. Implementar `UsuarioRepositoryEF` con Entity Framework
3. Configurar connection string
4. Crear migraciones
5. Cambiar el registro de servicios en `Program.cs`

¡Listo para conectar la base de datos cuando esté lista! 🎉
