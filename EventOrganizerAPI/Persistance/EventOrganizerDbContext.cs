using EventOrganizerAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;

namespace EventOrganizerAPI.Persistance
{
    public class EventOrganizerDbContext : DbContext
    {
        public EventOrganizerDbContext(DbContextOptions<EventOrganizerDbContext> options) : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .HasMany(u => u.AttendingEvents)
            .WithMany(e => e.Attendees)
            .UsingEntity(j => j.ToTable("EventAttendees"));

            modelBuilder.Entity<User>()
                .HasMany(u => u.OrganizedEvents)
                .WithOne(e => e.Organizer)
                .HasForeignKey(e => e.OrganizerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
        public DbSet<Event> Events { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Location> Locations { get; set; }
    }
}
