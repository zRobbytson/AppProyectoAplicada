using AppSistemaReservasRestaurente.Models;
using Microsoft.AspNetCore.Identity;

namespace AppSistemaReservasRestaurente.Data
{
    public static class BDInicio
    {
        public static async Task InicializarAsync(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var context = scope.ServiceProvider.GetRequiredService<BDContexto>();

                // 🔹 Crear roles si no existen
                string[] roles = { "Administrador", "Cliente" };
                foreach (var rol in roles)
                {
                    if (!await roleManager.RoleExistsAsync(rol))
                        await roleManager.CreateAsync(new IdentityRole<int>(rol));
                }

                // 🔹 Crear usuario Administrador
                var adminEmail = "admin@gmail.com";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);

                if (adminUser == null)
                {
                    adminUser = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail
                    };

                    var result = await userManager.CreateAsync(adminUser, "Admin1234!");
                    if (result.Succeeded)
                        await userManager.AddToRoleAsync(adminUser, "Administrador");
                }

                // 🔹 Crear usuario Cliente
                var clienteEmail = "cliente@gmail.com";
                var clienteUser = await userManager.FindByEmailAsync(clienteEmail);

                if (clienteUser == null)
                {
                    clienteUser = new ApplicationUser
                    {
                        UserName = clienteEmail,
                        Email = clienteEmail
                    };

                    var result = await userManager.CreateAsync(clienteUser, "Cliente1234!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(clienteUser, "Cliente");

                        // 👉 Crear registro en tabla Clientes
                        var cliente = new Cliente
                        {
                            Nombre_Cliente = "Cliente de Prueba",
                            Telefono = "987654321",
                            DNI = "12345678",
                            ID_Usuario = clienteUser.Id // FK con AspNetUsers
                        };

                        context.Clientes.Add(cliente);
                        await context.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
