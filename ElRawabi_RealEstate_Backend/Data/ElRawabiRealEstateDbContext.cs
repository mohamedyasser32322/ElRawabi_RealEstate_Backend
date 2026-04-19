using ElRawabi_RealEstate_Backend.Modals;
using Microsoft.EntityFrameworkCore;

namespace ElRawabi_RealEstate_Backend.Data
{
    public class ElRawabiRealEstateDbContext : DbContext
    {
        public ElRawabiRealEstateDbContext(DbContextOptions<ElRawabiRealEstateDbContext> options) : base(options) { }
        // Tables
        public DbSet<User> Users { get; set; }
        public DbSet<Buyer> Buyers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Floor> Floors { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<ConstructionStage> Stages { get; set; }
        public DbSet<BuildingImage> BuildingImages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Unique Fields
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Buyer>()
                .HasIndex(b => b.Email)
                .IsUnique();

            modelBuilder.Entity<Buyer>()
                .HasIndex(b => b.PhoneNumber)
                .IsUnique();

            modelBuilder.Entity<Buyer>()
                .HasIndex(b => b.NationalId)
                .IsUnique();

            modelBuilder.Entity<Project>()
                .HasIndex(p => p.Name)
                .IsUnique();

            modelBuilder.Entity<Role>()
                .HasIndex(r => r.RoleName)
                .IsUnique();

            modelBuilder.Entity<Unit>()
                .HasIndex(u => new { u.UnitNumber, u.FloorId })
                .IsUnique();

            modelBuilder.Entity<Unit>()
                .Property(u => u.Area)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Unit>()
                .Property(u => u.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Booking>()
                .Property(bo => bo.AmountPaid)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Booking>()
                .Property(bo => bo.RemainingAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Building>()
                .Property(b => b.Progress)
                .HasPrecision(18, 2);

            // Deleted Filter
            modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
            modelBuilder.Entity<Buyer>().HasQueryFilter(b => !b.IsDeleted);
            modelBuilder.Entity<Role>().HasQueryFilter(r => !r.IsDeleted);
            modelBuilder.Entity<Project>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<Building>().HasQueryFilter(bu => !bu.IsDeleted);
            modelBuilder.Entity<Floor>().HasQueryFilter(f => !f.IsDeleted);
            modelBuilder.Entity<Unit>().HasQueryFilter(un => !un.IsDeleted);
            modelBuilder.Entity<Booking>().HasQueryFilter(bo => !bo.IsDeleted);
            modelBuilder.Entity<ConstructionStage>().HasQueryFilter(s => !s.IsDeleted);
            modelBuilder.Entity<BuildingImage>().HasQueryFilter(i => !i.IsDeleted);
            modelBuilder.Entity<Notification>().HasQueryFilter(n => !n.IsDeleted);
            modelBuilder.Entity<ActivityLog>().HasQueryFilter(a => !a.IsDeleted);


            //Relationships
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ActivityLog>()
                .HasOne(a => a.User)
                .WithMany(u => u.ActivityLogs)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.RecipientUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Buyer)
                .WithMany(b => b.Notifications)
                .HasForeignKey(n => n.RecipientBuyerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Booking>()
                .HasOne(bo => bo.Buyer)
                .WithMany(b => b.Bookings)
                .HasForeignKey(bo => bo.BuyerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.Buildings)
                .WithOne(bu => bu.Project)
                .HasForeignKey(bu => bu.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Building>()
                .HasMany(bu => bu.Stages)
                .WithOne(s => s.Building)
                .HasForeignKey(s => s.BuildingId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Building>()
                .HasMany(bu => bu.Floors)
                .WithOne(f => f.Building)
                .HasForeignKey(f => f.BuildingId)
                .OnDelete(DeleteBehavior.Restrict);

            
            modelBuilder.Entity<Building>()
                .HasMany(bu => bu.Images)
                .WithOne(i => i.Building)
                .HasForeignKey(i => i.BuildingId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Floor>()
                .HasMany(f => f.Units)
                .WithOne(un => un.Floor)
                .HasForeignKey(un => un.FloorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Unit>()
                .HasOne(u => u.Booking)
                .WithOne(b => b.Unit)
                .HasForeignKey<Booking>(b => b.UnitId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}