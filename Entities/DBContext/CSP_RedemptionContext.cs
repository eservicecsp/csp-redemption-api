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
        public virtual DbSet<Brand> Brand { get; set; }
        public virtual DbSet<Campaign> Campaign { get; set; }
        public virtual DbSet<CampaignType> CampaignType { get; set; }
        public virtual DbSet<Consumer> Consumer { get; set; }
        public virtual DbSet<ConsumerSource> ConsumerSource { get; set; }
        public virtual DbSet<Dealer> Dealer { get; set; }
        public virtual DbSet<Function> Function { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Province> Province { get; set; }
        public virtual DbSet<QrCode> QrCode { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<RoleFunction> RoleFunction { get; set; }
        public virtual DbSet<Staff> Staff { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }
        public virtual DbSet<TransactionType> TransactionType { get; set; }
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

                entity.Property(e => e.NameEn).HasMaxLength(100);

                entity.Property(e => e.NameTh).HasMaxLength(100);

                entity.Property(e => e.ProvinceCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.HasOne(d => d.ProvinceCodeNavigation)
                    .WithMany(p => p.Amphur)
                    .HasForeignKey(d => d.ProvinceCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Amphur_Province");
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Campaign>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Campaign)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Campaign_Brand");

                entity.HasOne(d => d.CampaignType)
                    .WithMany(p => p.Campaign)
                    .HasForeignKey(d => d.CampaignTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Campaign_CampaignType");
            });

            modelBuilder.Entity<CampaignType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Consumer>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.AmphurCode).HasMaxLength(10);

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Latitude).HasMaxLength(50);

                entity.Property(e => e.Location).HasMaxLength(255);

                entity.Property(e => e.Longitude).HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.ProvinceCode).HasMaxLength(10);

                entity.Property(e => e.TumbolCode).HasMaxLength(10);

                entity.Property(e => e.ZipCode).HasMaxLength(5);

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Consumer)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Consumer_Brand");

                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.Consumer)
                    .HasForeignKey(d => d.CampaignId)
                    .HasConstraintName("FK_Consumer_Campaign");

                entity.HasOne(d => d.ConsumerSource)
                    .WithMany(p => p.Consumer)
                    .HasForeignKey(d => d.ConsumerSourceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Consumer_ConsumerSource");
            });

            modelBuilder.Entity<ConsumerSource>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Dealer>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Dealer)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dealer_Brand");
            });

            modelBuilder.Entity<Function>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Icon).HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Path).HasMaxLength(100);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Brand");
            });

            modelBuilder.Entity<Province>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.Property(e => e.Code)
                    .HasMaxLength(10)
                    .ValueGeneratedNever();

                entity.Property(e => e.NameEn).HasMaxLength(100);

                entity.Property(e => e.NameTh).HasMaxLength(100);

                entity.HasOne(d => d.Zone)
                    .WithMany(p => p.Province)
                    .HasForeignKey(d => d.ZoneId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Province_Zone");
            });

            modelBuilder.Entity<QrCode>(entity =>
            {
                entity.HasKey(e => new { e.Token, e.CampaignId });

                entity.Property(e => e.Token).HasMaxLength(50);

                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.QrCode)
                    .HasForeignKey(d => d.CampaignId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QrCode_Campaign");

                entity.HasOne(d => d.Consumer)
                    .WithMany(p => p.QrCode)
                    .HasForeignKey(d => d.ConsumerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QrCode_Consumer");

                entity.HasOne(d => d.Transaction)
                    .WithMany(p => p.QrCode)
                    .HasForeignKey(d => d.TransactionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_QrCode_Transaction");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<RoleFunction>(entity =>
            {
                entity.HasKey(e => new { e.RoleId, e.FunctionId });

                entity.HasOne(d => d.Function)
                    .WithMany(p => p.RoleFunction)
                    .HasForeignKey(d => d.FunctionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleFunction_Function");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleFunction)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleFunction_Role");
            });

            modelBuilder.Entity<Staff>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Staff)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Staff_Brand");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Staff)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Staff_Role");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.Transaction)
                    .HasForeignKey(d => d.CampaignId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transaction_Campaign");

                entity.HasOne(d => d.Consumer)
                    .WithMany(p => p.Transaction)
                    .HasForeignKey(d => d.ConsumerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transaction_Consumer");

                entity.HasOne(d => d.TransactionType)
                    .WithMany(p => p.Transaction)
                    .HasForeignKey(d => d.TransactionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transaction_TransactionType");
            });

            modelBuilder.Entity<TransactionType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
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

                entity.Property(e => e.NameEn).HasMaxLength(100);

                entity.Property(e => e.NameTh).HasMaxLength(100);

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

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.NameEn)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.NameTh)
                    .IsRequired()
                    .HasMaxLength(100);
            });
        }
    }
}
