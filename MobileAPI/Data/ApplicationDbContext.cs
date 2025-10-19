using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using MobileAPI.Models;
using System;
using System.Linq;
using System.Reflection.Emit;
using System.Threading;
using System.Threading.Tasks;

namespace MobileAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _config;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration config) : base(options)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Need a configuration object
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_config.GetConnectionString("DefaultConnection"));
            }
            // For Postgres only - requires EFCore.NamingConventions package
            optionsBuilder.UseSnakeCaseNamingConvention();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Create identity models if using IdentityDbContext
            base.OnModelCreating(builder);

            builder.Entity<HoursOfOperation>()
                .Property(h => h.Day)
                .HasConversion<string>();
        }

        public override int SaveChanges()
        {
            try
            {
                ManageTimestamp();
                return base.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                int result = RevertChanges(ex);
                if (result == 0)
                {
                    throw;
                }
                return result;
            }
        }

        public sealed override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return this.SaveChangesAsync(acceptAllChangesOnSuccess: true, cancellationToken);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            try
            {
                ManageTimestamp();
                return await base.SaveChangesAsync(true, cancellationToken).ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                int result = RevertChanges(ex);
                if (result == 0)
                {
                    throw;
                }
                return result;
            }
        }

        private void ManageTimestamp()
        {
            // Not everything will have a Timestamp field, hence being inside the try/catch & the continue
            ChangeTracker.DetectChanges();

            var entries = ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                bool hasTimestamp = entry.Properties.Any(x => x.Metadata.Name.Equals("Timestamp"));

                if (hasTimestamp)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Property("Timestamp").CurrentValue = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    }
                    else
                    {
                        long stamp = (long)entry.Property("Timestamp").CurrentValue;
                        long dbstamp = (long)entry.Property("Timestamp").OriginalValue;
                        if (stamp != dbstamp)
                        {
                            throw new DbUpdateConcurrencyException("Timestamps of " + entry.ToString() + " does not match database value");
                        }
                        else
                        {
                            entry.Property("Timestamp").CurrentValue = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                        }
                    }
                }
            }
        }

        private int RevertChanges(DbUpdateConcurrencyException ex)
        {
            // Reset changes
            foreach (var entry in ex.Entries)
            {
                // If this is a new entity, it won't have a database record
                if (entry.State != EntityState.Added)
                {
                    var proposedValues = entry.CurrentValues;
                    var databaseValues = entry.GetDatabaseValues();

                    bool identicalValues = true;

                    foreach (var property in proposedValues.Properties)
                    {
                        var proposedValue = proposedValues[property];
                        var databaseValue = databaseValues[property];

                        if (!proposedValue.Equals(databaseValue))
                        {
                            identicalValues = false;
                            break;
                        }
                    }

                    if (identicalValues) // Values were reassigned the same values, false alarm
                    {
                        return base.SaveChanges();
                    }

                    // Refresh original values to bypass next concurrency check
                    entry.OriginalValues.SetValues(databaseValues);
                }
            }

            return 0;
        }

        // Don't forget to add a migration when changing these
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<BusinessOffice> BusinessOffices { get; set; }
        public DbSet<CafeteriaMenu> CafeteriaMenus { get; set; }
        public DbSet<Campus> Campuses { get; set; }
        public DbSet<HoursOfOperation> HoursOfOperation { get; set; }
        public DbSet<Library> Librarys { get; set; }
        public DbSet<Registrar> Registrars { get; set; }
        public DbSet<School> Schools { get; set; }
        
        public DbSet<SchoolEvent> SchoolEvents { get; set; }
        public DbSet<SchoolNews> SchoolNews { get; set; }
        public DbSet<Security> Securitys { get; set; }
        public DbSet<SecurityAlert> SecurityAlerts { get; set; }
        public DbSet<TutoringCenter> TutoringCenters { get; set; }
        public DbSet<UserPreference> UserPreferences { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
    }
}