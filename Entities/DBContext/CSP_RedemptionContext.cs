using System;
using System.IO;
using CSP_Redemption_WebApi.Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;


namespace CSP_Redemption_WebApi.Entities.DBContext
{
    public partial class CSP_RedemptionContext : DbContext
    {
        public CSP_RedemptionContext()
        {
        }

        public CSP_RedemptionContext(DbContextOptions<CSP_RedemptionContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Amphur> Amphur { get; set; }
        public virtual DbSet<Campaign> Campaign { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<Consumer> Consumer { get; set; }
        public virtual DbSet<ConsumerType> ConsumerType { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
        public virtual DbSet<Province> Province { get; set; }
        public virtual DbSet<QrCode> QrCode { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<RoleMenu> RoleMenu { get; set; }
        public virtual DbSet<Staff> Staff { get; set; }
        public virtual DbSet<Tumbol> Tumbol { get; set; }
        public virtual DbSet<Zone> Zone { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                                                                             .AddJsonFile("appsettings.json")
                                                                                                             .Build();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("APIDatabase"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<Amphur>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.Property(e => e.Code)
                    .HasMaxLength(10)
                    .ValueGeneratedNever();

                entity.Property(e => e.NameEn).HasMaxLength(150);

                entity.Property(e => e.NameTh).HasMaxLength(150);

                entity.Property(e => e.ProvinceCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.HasOne(d => d.ProvinceCodeNavigation)
                    .WithMany(p => p.Amphur)
                    .HasForeignKey(d => d.ProvinceCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Amphur_Province");
            });

            modelBuilder.Entity<Campaign>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Campaign)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Campaign_Company");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<Consumer>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AmphurCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Latitude).HasMaxLength(50);

                entity.Property(e => e.Location).HasMaxLength(255);

                entity.Property(e => e.Longitude).HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.ProvinceCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.TumbolCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.ZipCode)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.HasOne(d => d.AmphurCodeNavigation)
                    .WithMany(p => p.Consumer)
                    .HasForeignKey(d => d.AmphurCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Consumer_Amphur");

                entity.HasOne(d => d.ConsumerType)
                    .WithMany(p => p.Consumer)
                    .HasForeignKey(d => d.ConsumerTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Consumer_ConsumerType");

                entity.HasOne(d => d.ProvinceCodeNavigation)
                    .WithMany(p => p.Consumer)
                    .HasForeignKey(d => d.ProvinceCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Consumer_Province");

                entity.HasOne(d => d.TumbolCodeNavigation)
                    .WithMany(p => p.Consumer)
                    .HasForeignKey(d => d.TumbolCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Consumer_Tumbol");
            });

            modelBuilder.Entity<ConsumerType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description).IsRequired();
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Icon).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Province>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.Property(e => e.Code)
                    .HasMaxLength(10)
                    .ValueGeneratedNever();

                entity.Property(e => e.NameEn).HasMaxLength(150);

                entity.Property(e => e.NameTh).HasMaxLength(150);

                entity.HasOne(d => d.Zone)
                    .WithMany(p => p.Province)
                    .HasForeignKey(d => d.ZoneId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Province_Zone");
            });

            modelBuilder.Entity<QrCode>(entity =>
            {
                entity.HasKey(e => new { e.QrCode1, e.CampaignId })
                    .HasName("PK_Code");

                entity.Property(e => e.QrCode1)
                    .HasColumnName("QrCode")
                    .HasMaxLength(42)
                    .IsUnicode(false);

                entity.Property(e => e.ScanDate).HasColumnType("datetime");

                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.QrCode)
                    .HasForeignKey(d => d.CampaignId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Code_Campaign");

                entity.HasOne(d => d.Consumer)
                    .WithMany(p => p.QrCode)
                    .HasForeignKey(d => d.ConsumerId)
                    .HasConstraintName("FK_QrCode_Consumer");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Role)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_Role_Company");
            });

            modelBuilder.Entity<RoleMenu>(entity =>
            {
                entity.HasKey(e => new { e.RoleId, e.MenuId });

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.RoleMenu)
                    .HasForeignKey(d => d.MenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleMenu_Menu");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleMenu)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleMenu_Role");
            });

            modelBuilder.Entity<Staff>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Staff)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Staff_Company");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Staff)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Staff_Role");
            });

            modelBuilder.Entity<Tumbol>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.Property(e => e.Code)
                    .HasMaxLength(10)
                    .ValueGeneratedNever();

                entity.Property(e => e.AmphurCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.NameEn).HasMaxLength(150);

                entity.Property(e => e.NameTh).HasMaxLength(150);

                entity.Property(e => e.ZipCode).HasMaxLength(5);

                entity.HasOne(d => d.AmphurCodeNavigation)
                    .WithMany(p => p.Tumbol)
                    .HasForeignKey(d => d.AmphurCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tumbol_Amphur");
            });

            modelBuilder.Entity<Zone>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.NameEn).HasMaxLength(150);

                entity.Property(e => e.NameTh).HasMaxLength(150);
            });
        }
    }
}
