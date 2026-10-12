using Microsoft.EntityFrameworkCore;
using PetCareAPI.Domain.Models;
using PetCareAPI.Repository.Outbox;

namespace PetCareAPI.Repository;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Pet> Pets { get; set; }
    
    public DbSet<Appointment> Appointments { get; set; }
    
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pet>(entity =>
        {
            entity.HasKey(x => x.PetId);
            
            entity.HasMany(x => x.Appointments)
                .WithOne()
                .HasForeignKey(x => x.PetId);
            
            entity.Property(x => x.PetId)
                .ValueGeneratedNever();
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(x => x.AppointmentId);

            entity.Property(x => x.AppointmentId)
                .ValueGeneratedNever();
        });
        
        modelBuilder.Entity<OutboxMessage>(entity =>
        {
            entity.HasKey(x => x.MessageId);

            entity.Property(x => x.MessageId)
                .ValueGeneratedNever();
        });
    }
}