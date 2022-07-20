using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TicketWebApi.Models
{
    public partial class TicketDbContext : DbContext
    {
        public TicketDbContext()
        {
        }

        public TicketDbContext(DbContextOptions<TicketDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<HistorialIncidencium> HistorialIncidencia { get; set; } = null!;
        public virtual DbSet<Ticket> Tickets { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=ConnectionStrings:Chinook");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HistorialIncidencium>(entity =>
            {
                entity.HasKey(e => e.HistorialAtencionId)
                    .HasName("NewTable_PK");

                entity.Property(e => e.Comentario)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.FechaAtencion)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UsuarioSoporte)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.HistorialIncidencia)
                    .HasForeignKey(d => d.TicketId)
                    .HasConstraintName("HistorialIncidencia_FK");
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.ToTable("Ticket");

                entity.Property(e => e.Asunto)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(75)
                    .IsUnicode(false);

                entity.Property(e => e.FechaIngreso)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.PersonaSolicitante)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
