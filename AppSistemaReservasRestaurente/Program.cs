using AppSistemaReservasRestaurente.Data;
using AppSistemaReservasRestaurente.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class Program
{
    public static async Task Main(string[] args) // 🔹 ahora es async
    {
        var builder = WebApplication.CreateBuilder(args);

        // Servicios
        builder.Services.AddControllersWithViews();

        // 🔹 Conexión a la BD
        builder.Services.AddDbContext<BDContexto>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("BDContexto")
                ?? throw new InvalidOperationException("Cadena de conexión 'BDContexto' no existe.")));

        // 🔹 Configuración de Identity
        builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 4;

            // ❌ QUITA la confirmación de cuenta
            options.SignIn.RequireConfirmedAccount = false;
        })
        .AddEntityFrameworkStores<BDContexto>()
        .AddDefaultUI()
        .AddDefaultTokenProviders();

        // 🔹 Configuración de cookies (ruta del login/logout)
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Identity/Account/Login";
            options.LogoutPath = "/Identity/Account/Logout";
            options.AccessDeniedPath = "/Identity/Account/AccessDenied";
        });

        builder.Services.AddRazorPages(); // 🔹 necesario para Identity

        var app = builder.Build();

        // Middlewares
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        using (var scope = app.Services.CreateScope())
        {
            var servicio = scope.ServiceProvider;
            var contexto = servicio.GetRequiredService<BDContexto>();
            contexto.Database.EnsureCreated();

            // 🔹 Inicializa roles y usuarios
            await BDInicio.InicializarAsync(servicio);
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        // 🔹 Mapear rutas
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        // 🔹 Identity
        app.MapRazorPages();

        await app.RunAsync(); // 👈 ahora también es async
    }
}
