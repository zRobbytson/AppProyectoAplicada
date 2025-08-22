using AppSistemaReservasRestaurente.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<BDContexto>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BDContexto") ?? throw new InvalidOperationException("Cadena de conexion 'conexionSqlServer' no existe.")));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var servicio = scope.ServiceProvider;
    var contexto = servicio.GetRequiredService<BDContexto>();
    contexto.Database.EnsureCreated(); // Asegura que la base de datos se crea si no existe
    /*BDInicio.Inicializar(contexto);*/ // Inicializa la base de datos con datos predeterminados
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
