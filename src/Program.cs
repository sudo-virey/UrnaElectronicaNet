using Microsoft.EntityFrameworkCore;
using UrnaElectronica.Services;
using UrnaElectronica.Data;
using UrnaElectronica.Middleware;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIGURACIÓN DE BASE DE DATOS ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// --- 2. REGISTRO DE SERVICIOS (Dependency Injection) ---
builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepositoryEF>(); 
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// --- 3. PRUEBA DE CONEXIÓN (Health Check) ---
// Ponemos esto aquí para que imprima el estado en la consola al arrancar
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        if (context.Database.CanConnect())
        {
            Console.WriteLine("\n✅ [DATABASE] Connection established successfully!");
        }
        else
        {
            Console.WriteLine("\n❌ [DATABASE] Connection failed. Check your connection string.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\n❌ [DATABASE] Critical Error: {ex.Message}");
    }
}

// --- 4. PIPELINE DE LA APLICACIÓN (Middleware) ---
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();
app.UseMiddleware<AutenticacionMiddleware>();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Esta siempre es la última línea
app.Run();