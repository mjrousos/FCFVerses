using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Data.Models;

namespace WebApi.Data
{
    public class VersesDbContext : DbContext
    {
        public DbSet<Group> Groups { get; set; } = default!;

        public DbSet<GroupRole> GroupRoles { get; set; } = default!;

        public DbSet<PassageReference> PassageReferences { get; set; } = default!;

        public DbSet<PassageReference> UserSettings { get; set; } = default!;

        public DbSet<Verse> Verses { get; set; } = default!;

        public VersesDbContext(DbContextOptions<VersesDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Group configuration
            modelBuilder.Entity<Group>()
                .Property(g => g.Name)
                .HasMaxLength(255)
                .IsRequired();

            modelBuilder.Entity<Group>()
                .HasMany(g => g.GroupRoles)
                .WithOne(r => r.Group)
                .HasForeignKey(r => r.GroupId)
                .IsRequired();

            modelBuilder.Entity<Group>()
                .HasMany(g => g.PassageReferences)
                .WithOne();

            // Group role configuration
            modelBuilder.Entity<GroupRole>()
                .HasKey(r => new { r.UserSettingsId, r.GroupId });

            modelBuilder.Entity<GroupRole>()
                .HasOne(r => r.UserSettings)
                .WithMany(s => s.GroupRoles)
                .HasForeignKey(x => x.UserSettingsId)
                .IsRequired();

            modelBuilder.Entity<GroupRole>()
                .HasOne(r => r.Group)
                .WithMany(g => g.GroupRoles)
                .HasForeignKey(r => r.GroupId)
                .IsRequired();

            modelBuilder.Entity<GroupRole>()
                .Property(r => r.Role)
                .IsRequired();

            // PassageReference configuration
            modelBuilder.Entity<PassageReference>()
                .Ignore(p => p.Verses);

            // UserSettings configuration
            modelBuilder.Entity<UserSettings>()
                .Property(s => s.UserId)
                .HasMaxLength(50)
                .IsRequired();

            modelBuilder.Entity<UserSettings>()
                .HasMany(s => s.GroupRoles)
                .WithOne(r => r.UserSettings)
                .HasForeignKey(r => r.UserSettingsId)
                .IsRequired();

            // Verse configuration
            modelBuilder.Entity<Verse>()
                .Property(v => v.Text)
                .HasMaxLength(255)
                .IsRequired();
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var updateTime = DateTimeOffset.Now;
            foreach (var change in ChangeTracker.Entries<IEntity>())
            {
                switch (change.State)
                {
                    case EntityState.Added:
                        change.Entity.CreatedDate = change.Entity.LastModifiedDate = updateTime;
                        break;
                    case EntityState.Modified:
                        change.Entity.LastModifiedDate = updateTime;
                        break;
                }
            }
        }
    }
}
