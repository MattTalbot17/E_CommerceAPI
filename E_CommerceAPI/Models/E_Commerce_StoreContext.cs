using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace E_Commerce_API.Models
{
    public partial class E_Commerce_StoreContext : DbContext
    {
        public E_Commerce_StoreContext()
        {
        }

        public E_Commerce_StoreContext(DbContextOptions<E_Commerce_StoreContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public virtual DbSet<LogSingleton> LogSingleton { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Sale> Sale { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customers>(entity =>
            {
                entity.HasKey(x => x.CustomerId)
                    .HasName("PK__Customer__8CB286B9105C9E9D");

                entity.Property(e => e.CustomerId).HasColumnName("Customer_ID");

                entity.Property(e => e.Province)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.StreetName)
                    .IsRequired()
                    .HasColumnName("Street_Name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.StreetNumber).HasColumnName("Street_Number");

                entity.Property(e => e.Suburb)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Customers__User___1273C1CD");
            });

            modelBuilder.Entity<Employees>(entity =>
            {
                entity.HasKey(x => x.EmployeeId)
                    .HasName("PK__Employee__78113481FF63E2FA");

                entity.Property(e => e.EmployeeId).HasColumnName("Employee_ID");

                entity.Property(e => e.EmployeePosition)
                    .IsRequired()
                    .HasColumnName("Employee_Position")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Employees__User___15502E78");
            });

            modelBuilder.Entity<LogSingleton>(entity =>
            {
                entity.HasKey(x => x.LogId)
                    .HasName("PK__Log_Sing__2D26E7AEE7D64F59");

                entity.ToTable("Log_Singleton");

                entity.Property(e => e.LogId).HasColumnName("Log_ID");

                entity.Property(e => e.LogDate)
                    .HasColumnName("Log_Date")
                    .HasColumnType("date");

                entity.Property(e => e.LogType)
                    .IsRequired()
                    .HasColumnName("Log_Type")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ProductId).HasColumnName("Product_ID");

                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.LogSingleton)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Log_Singl__User___1DE57479");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductId).HasColumnName("Product_ID");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.ProductCategory)
                    .IsRequired()
                    .HasColumnName("Product_Category")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ProductDescription)
                    .IsRequired()
                    .HasColumnName("Product_Description")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ProductImage)
                    .IsRequired()
                    .HasColumnName("Product_Image")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasColumnName("Product_Name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ProductPrice).HasColumnName("Product_Price");
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.Property(e => e.SaleId).HasColumnName("Sale_ID");

                entity.Property(e => e.DateOfSale)
                    .HasColumnName("Date_Of_Sale")
                    .HasColumnType("date");

                entity.Property(e => e.ProductId).HasColumnName("Product_ID");

                entity.Property(e => e.UserId).HasColumnName("User_ID");


                entity.HasOne(d => d.User)
                    .WithMany(p => p.Sale)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Sale__User_ID__1A14E395");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(x => x.UserId)
                    .HasName("PK__Users__206D9190C04B734B");

                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("First_Name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("Last_Name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
