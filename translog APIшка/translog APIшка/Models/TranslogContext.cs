using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace translog_APIшка.Models;

public partial class TranslogContext : DbContext
{
    public TranslogContext()
    {
    }

    public TranslogContext(DbContextOptions<TranslogContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Driver> Drivers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Trip> Trips { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vehicle> Vehicles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;database=translog", Microsoft.EntityFrameworkCore.ServerVersion.Parse("11.8.6-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_unicode_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Driver>(entity =>
        {
            entity.HasKey(e => e.DriverId).HasName("PRIMARY");

            entity.ToTable("drivers");

            entity.HasIndex(e => e.UserId, "users_idx");

            entity.Property(e => e.DriverId)
                .HasColumnType("int(11)")
                .HasColumnName("driver_id");
            entity.Property(e => e.FullName)
                .HasMaxLength(150)
                .HasColumnName("full_name");
            entity.Property(e => e.LicensePlate)
                .HasMaxLength(255)
                .HasColumnName("license_plate");
            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Drivers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("users");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PRIMARY");

            entity.ToTable("orders");

            entity.HasIndex(e => e.UserId, "fk_orders_users");

            entity.Property(e => e.OrderId)
                .HasColumnType("int(11)")
                .HasColumnName("order_id");
            entity.Property(e => e.ArrivalPoint)
                .HasMaxLength(255)
                .HasColumnName("arrival_point");
            entity.Property(e => e.DeparturePoint)
                .HasMaxLength(255)
                .HasColumnName("departure_point");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasColumnName("full_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(40)
                .HasColumnName("phone");
            entity.Property(e => e.ReceivedAt)
                .HasColumnType("datetime")
                .HasColumnName("received_at");
            entity.Property(e => e.Status)
                .HasMaxLength(100)
                .HasColumnName("status");
            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("user_id");
            entity.Property(e => e.VolumeM3)
                .HasPrecision(10, 3)
                .HasColumnName("volume_m3");
            entity.Property(e => e.Weight)
                .HasPrecision(8, 2)
                .HasColumnName("weight");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_orders_users");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PRIMARY");

            entity.ToTable("roles");

            entity.Property(e => e.RoleId)
                .HasColumnType("int(11)")
                .HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(255)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<Trip>(entity =>
        {
            entity.HasKey(e => e.TripId).HasName("PRIMARY");

            entity.ToTable("trips");

            entity.HasIndex(e => e.VehicleId, "fk_trips_vehicles");

            entity.Property(e => e.TripId)
                .HasColumnType("int(11)")
                .HasColumnName("trip_id");
            entity.Property(e => e.ArrivalPoint)
                .HasMaxLength(100)
                .HasColumnName("arrival_point");
            entity.Property(e => e.ArrivalTime)
                .HasColumnType("datetime")
                .HasColumnName("arrival_time");
            entity.Property(e => e.DeparturePoint)
                .HasMaxLength(100)
                .HasColumnName("departure_point");
            entity.Property(e => e.DepartureTime)
                .HasColumnType("datetime")
                .HasColumnName("departure_time");
            entity.Property(e => e.DistanceKm)
                .HasPrecision(10, 2)
                .HasColumnName("distance_km");
            entity.Property(e => e.DriverId)
                .HasColumnType("int(11)")
                .HasColumnName("driver_id");
            entity.Property(e => e.OrderId)
                .HasColumnType("int(11)")
                .HasColumnName("order_id");
            entity.Property(e => e.VehicleId)
                .HasColumnType("int(11)")
                .HasColumnName("vehicle_id");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.Trips)
                .HasForeignKey(d => d.VehicleId)
                .HasConstraintName("fk_trips_vehicles");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.RoleId, "role_idx");

            entity.HasIndex(e => e.Username, "username").IsUnique();

            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("user_id");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("'1'")
                .HasColumnType("int(11)")
                .HasColumnName("is_active");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.RoleId)
                .HasColumnType("int(11)")
                .HasColumnName("role_id");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("role");
        });

        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.VehicleId).HasName("PRIMARY");

            entity.ToTable("vehicles");

            entity.HasIndex(e => e.DriverId, "fk_vehicles_drivers");

            entity.HasIndex(e => e.LicensePlate, "uq_vehicles_plate").IsUnique();

            entity.Property(e => e.VehicleId)
                .HasColumnType("int(11)")
                .HasColumnName("vehicle_id");
            entity.Property(e => e.BaseRatePerKm)
                .HasPrecision(10, 2)
                .HasColumnName("base_rate_per_km");
            entity.Property(e => e.Brand)
                .HasMaxLength(45)
                .HasColumnName("brand");
            entity.Property(e => e.DriverId)
                .HasColumnType("int(11)")
                .HasColumnName("driver_id");
            entity.Property(e => e.LicensePlate).HasColumnName("license_plate");
            entity.Property(e => e.LoadCapacity)
                .HasPrecision(8, 2)
                .HasColumnName("load_capacity");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");

            entity.HasOne(d => d.Driver).WithMany(p => p.Vehicles)
                .HasForeignKey(d => d.DriverId)
                .HasConstraintName("fk_vehicles_drivers");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
