using AppSistemaReservasRestaurente.Models;
using Microsoft.EntityFrameworkCore;

namespace AppSistemaReservasRestaurente.Data
{
    public class BDContexto : DbContext
    {
        public BDContexto(DbContextOptions<BDContexto> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes { get; set; } = default!;
        public DbSet<Mesa> Mesas { get; set; } = default!;
        public DbSet<Horario> Horarios { get; set; } = default!;
        public DbSet<Reserva> Reservas { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Mesa>()
                .HasIndex(m => m.Numero)
                .IsUnique();

            builder.Entity<Reserva>()
                .HasOne(r => r.Cliente)
                .WithMany(c => c.Reservas)
                .HasForeignKey(r => r.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Reserva>()
                .HasOne(r => r.Mesa)
                .WithMany(m => m.Reservas)
                .HasForeignKey(r => r.MesaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Reserva>()
                .HasOne(r => r.Horario)
                .WithMany(h => h.Reservas)
                .HasForeignKey(r => r.HorarioId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
