using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DigitalApp2.Models;

public partial class DigitalApp1Context : DbContext
{
    public DigitalApp1Context()
    {
    }

    public DigitalApp1Context(DbContextOptions<DigitalApp1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("name=Conn");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C0D2C65EC");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.EmailAddress).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(40);
            entity.Property(e => e.UserStatus).HasDefaultValue(true);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
