using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using System.Text.Json;

namespace backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeProperty> EmployeeProperties { get; set; }
        public DbSet<EmployeePropertyValue> EmployeePropertyValues { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var listToJson = new ValueConverter<ICollection<string>?, string?>(
                v => v == null ? null : JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => v == null ? null : JsonSerializer.Deserialize<ICollection<string>>(v, (JsonSerializerOptions?)null)!
            );

            modelBuilder.Entity<EmployeeProperty>()
                .Property(p => p.DropdownOptions)
                .HasConversion(listToJson)
                .HasMaxLength(4000);

            modelBuilder.Entity<EmployeePropertyValue>()
                .HasOne(v => v.Employee).WithMany(e => e.PropertyValues)
                .HasForeignKey(v => v.EmployeeId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EmployeePropertyValue>()
                .HasOne(v => v.Property).WithMany()
                .HasForeignKey(v => v.PropertyId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
