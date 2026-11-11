using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MusicSchool.Models;

namespace MusicSchool.Data;

public partial class MusicSchoolContext : DbContext
{
    public MusicSchoolContext(DbContextOptions<MusicSchoolContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; } = null!;
    public virtual DbSet<Group> Groups { get; set; } = null!;
    public virtual DbSet<Order> Orders { get; set; } = null!;
    public virtual DbSet<OrderItem> OrderItems { get; set; } = null!;
    public virtual DbSet<CourseProgram> Programs { get; set; } = null!; 
    public virtual DbSet<Role> Roles { get; set; } = null!;
    public virtual DbSet<Student> Students { get; set; } = null!;
    public virtual DbSet<Teacher> Teachers { get; set; } = null!;
    public virtual DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__categori__D54EE9B4EE4C7C92");
            entity.ToTable("categories");
            entity.HasIndex(e => e.CategoryName, "UQ__categori__5189E2558062F244").IsUnique();
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryName).HasMaxLength(100).HasColumnName("category_name");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.GroupId).HasName("PK__groups__D57795A0BB1AAD26");
            entity.ToTable("groups");
            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.AvailableSeats).HasColumnName("available_seats");
            entity.Property(e => e.GroupName).HasMaxLength(100).HasColumnName("group_name");
            entity.Property(e => e.ProgramId).HasColumnName("program_id");
            entity.Property(e => e.Schedule).HasMaxLength(200).HasColumnName("schedule");
            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");

            entity.HasOne(d => d.CourseProgram).WithMany(p => p.Groups)
                .HasForeignKey(d => d.ProgramId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__groups__program___4E53A1AA");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Groups)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__groups__teacher___4F47C5E3");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__orders__46596229D875E743");
            entity.ToTable("orders");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(CONVERT([date],getdate()))", "df_orders_date")
                .HasColumnName("order_date");
            entity.Property(e => e.StudentId).HasColumnName("student_id");

            entity.HasOne(d => d.Student).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__orders__student___55009F39");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.OrderItemId).HasName("PK__order_it__3764B6BCA985C9E1");
            entity.ToTable("order_items");
            entity.Property(e => e.OrderItemId).HasColumnName("order_item_id");
            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.UnitPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("unit_price");

            entity.HasOne(d => d.Group).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__order_ite__group__58D1301D");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__order_ite__order__57DD0BE4");
        });

        modelBuilder.Entity<CourseProgram>(entity =>
        {
            entity.HasKey(e => e.ProgramId).HasName("PK__programs__3A7890AC306A3AE8");
            entity.ToTable("programs");
            entity.Property(e => e.ProgramId).HasColumnName("program_id");
            entity.Property(e => e.BasePrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("base_price");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ProgramName)
                .HasMaxLength(150)
                .HasColumnName("program_name");

            entity.HasOne(d => d.Category).WithMany(p => p.Programs)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__programs__catego__4A8310C6");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__roles__760965CC8238564E");
            entity.ToTable("roles");
            entity.HasIndex(e => e.RoleName, "UQ__roles__783254B1454480E7").IsUnique();
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("role_name");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.StudentId).HasName("PK__students__2A33069A10391F1E");
            entity.ToTable("students");
            entity.HasIndex(e => e.UserId, "UQ__students__B9BE370EFF4895D3").IsUnique();
            entity.Property(e => e.StudentId).HasColumnName("student_id");
            entity.Property(e => e.BirthDate).HasColumnName("birth_date");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithOne(p => p.Student)
                .HasForeignKey<Student>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__students__user_i__40F9A68C");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.TeacherId).HasName("PK__teachers__03AE777E77DC9F9F");
            entity.ToTable("teachers");
            entity.HasIndex(e => e.UserId, "UQ__teachers__B9BE370E21619D91").IsUnique();
            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.Specialization)
                .HasMaxLength(100)
                .HasColumnName("specialization");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithOne(p => p.Teacher)
                .HasForeignKey<Teacher>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__teachers__user_i__44CA3770");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__users__B9BE370FAB12D527");
            entity.ToTable("users");
            entity.HasIndex(e => e.Login, "UQ__users__7838F2725FAF8987").IsUnique();
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(60)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(60)
                .HasColumnName("last_name");
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .HasColumnName("login");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(60)
                .HasColumnName("middle_name");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("role_id");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__users__role_id__3D2915A8");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
