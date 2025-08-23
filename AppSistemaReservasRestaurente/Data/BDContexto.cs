using AppSistemaReservasRestaurente.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AppSistemaReservasRestaurente.Data
{
    public class BDContexto : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public BDContexto(DbContextOptions<BDContexto> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; } = default!;
        public DbSet<Mesa> Mesas { get; set; } = default!;
        public DbSet<Horario> Horarios { get; set; } = default!;
        public DbSet<Reserva> Reservas { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // --- Relación Usuario - Cliente (1:1) ---
            builder.Entity<Cliente>()
                .HasOne(c => c.Usuario)
                .WithOne(u => u.Cliente)
                .HasForeignKey<Cliente>(c => c.Id)
                .OnDelete(DeleteBehavior.Cascade);

            // --- Relación Reserva - Cliente ---
            builder.Entity<Reserva>()
                .HasOne(r => r.Cliente)
                .WithMany(c => c.Reservas)
                .HasForeignKey(r => r.ID_Cliente)
                .OnDelete(DeleteBehavior.Restrict);

            // --- Relación Reserva - Mesa ---
            builder.Entity<Reserva>()
                .HasOne(r => r.Mesa)
                .WithMany(m => m.Reservas)
                .HasForeignKey(r => r.ID_Mesa)
                .OnDelete(DeleteBehavior.Restrict);

            // --- Relación Reserva - Horario ---
            builder.Entity<Reserva>()
                .HasOne(r => r.Horario)
                .WithMany(h => h.Reservas)
                .HasForeignKey(r => r.ID_Horario)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
