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
        public virtual DbSet<CampaignDealer> CampaignDealer { get; set; }
        public virtual DbSet<CampaignProduct> CampaignProduct { get; set; }
        public virtual DbSet<CampaignType> CampaignType { get; set; }
        public virtual DbSet<Collection> Collection { get; set; }
        public virtual DbSet<Consumer> Consumer { get; set; }
        public virtual DbSet<ConsumerProductType> ConsumerProductType { get; set; }
        public virtual DbSet<ConsumerSource> ConsumerSource { get; set; }
        public virtual DbSet<Dealer> Dealer { get; set; }
        public virtual DbSet<Enrollment> Enrollment { get; set; }
        public virtual DbSet<Function> Function { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductAttachment> ProductAttachment { get; set; }
        public virtual DbSet<ProductType> ProductType { get; set; }
        public virtual DbSet<Promotion> Promotion { get; set; }
        public virtual DbSet<PromotionType> PromotionType { get; set; }
        public virtual DbSet<Province> Province { get; set; }
        public virtual DbSet<QrCode> QrCode { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<RoleFunction> RoleFunction { get; set; }
        public virtual DbSet<Staff> Staff { get; set; }
        public virtual DbSet<Theme> Theme { get; set; }
        public virtual DbSet<ThemeConfig> ThemeConfig { get; set; }
        public virtual DbSet<Tracking> Tracking { get; set; }
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
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("ApiDatabase"));
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
                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(5);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Campaign>(entity =>
            {
                entity.Property(e => e.AlertMessage).HasMaxLength(255);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.DuplicateMessage).HasMaxLength(255);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.QrCodeNotExistMessage).HasMaxLength(255);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.Url).HasMaxLength(255);

                entity.Property(e => e.WinMessage).HasMaxLength(255);

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

                entity.HasOne(d => d.Dealer)
                    .WithMany(p => p.Campaign)
                    .HasForeignKey(d => d.DealerId)
                    .HasConstraintName("FK_Campaign_Dealer");
            });

            modelBuilder.Entity<CampaignDealer>(entity =>
            {
                entity.HasKey(e => new { e.CampaignId, e.DealerId });

                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.CampaignDealer)
                    .HasForeignKey(d => d.CampaignId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CampaignDealer_Campaign");

                entity.HasOne(d => d.Dealer)
                    .WithMany(p => p.CampaignDealer)
                    .HasForeignKey(d => d.DealerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CampaignDealer_Dealer");
            });

            modelBuilder.Entity<CampaignProduct>(entity =>
            {
                entity.HasKey(e => new { e.CampaignId, e.ProductId });

                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.CampaignProduct)
                    .HasForeignKey(d => d.CampaignId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CampaignProduct_Campaign");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.CampaignProduct)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CampaignProduct_Product");
            });

            modelBuilder.Entity<CampaignType>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.ImagePath).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.SubTitle).HasMaxLength(100);

                entity.Property(e => e.Title).HasMaxLength(100);
            });

            modelBuilder.Entity<Collection>(entity =>
            {
                entity.Property(e => e.CollectionName).HasMaxLength(255);

                entity.Property(e => e.Extension).HasMaxLength(255);

                entity.HasOne(d => d.Campaign)
                    .WithMany(p => p.Collection)
                    .HasForeignKey(d => d.CampaignId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Collection_Campaign");
            });

            modelBuilder.Entity<Consumer>(entity =>
            {
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

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.ProvinceCode).HasMaxLength(10);

                entity.Property(e => e.TumbolCode).HasMaxLength(10);

                entity.Property(e => e.ZipCode).HasMaxLength(5);

                entity.HasOne(d => d.AmphurCodeNavigation)
                    .WithMany(p => p.Consumer)
                    .HasForeignKey(d => d.AmphurCode)
                    .HasConstraintName("FK_Consumer_Amphur");

                entity.HasOne(d => d.ProvinceCodeNavigation)
                    .WithMany(p => p.Consumer)
                    .HasForeignKey(d => d.ProvinceCode)
                    .HasConstraintName("FK_Consumer_Province");

                entity.HasOne(d => d.TumbolCodeNavigation)
                    .WithMany(p => p.Consumer)
                    .HasForeignKey(d => d.TumbolCode)
                    .HasConstraintName("FK_Consumer_Tumbol");
            });

            modelBuilder.Entity<ConsumerProductType>(entity =>
            {
                entity.HasKey(e => new { e.ProductTypeId, e.ConsumerId });

                entity.HasOne(d => d.Consumer)
                    .WithMany(p => p.ConsumerProductType)
                    .HasForeignKey(d => d.ConsumerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConsumerProductType_Consumer");

                entity.HasOne(d => d.ProductType)
                    .WithMany(p => p.ConsumerProductType)
                    .HasForeignKey(d => d.ProductTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConsumerProductType_ProductType");
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
                entity.Property(e => e.AmphurCode).HasMaxLength(10);

                entity.Property(e => e.BranchNo).HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Phone).HasMaxLength(10);

                entity.Property(e => e.ProvinceCode).HasMaxLength(10);

                entity.Property(e => e.TaxNo).HasMaxLength(15);

                entity.Property(e => e.Tel).HasMaxLength(20);

                entity.Property(e => e.TumbolCode).HasMaxLength(10);

                entity.Property(e => e.ZipCode).HasMaxLength(5);

                entity.HasOne(d => d.AmphurCodeNavigation)
                    .WithMany(p => p.Dealer)
                    .HasForeignKey(d => d.AmphurCode)
                    .HasConstraintName("FK_Dealer_Amphur");

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Dealer)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Dealer_Brand");

                entity.HasOne(d => d.ProvinceCodeNavigation)
                    .WithMany(p => p.Dealer)
                    .HasForeignKey(d => d.ProvinceCode)
                    .HasConstraintName("FK_Dealer_Province");

                entity.HasOne(d => d.TumbolCodeNavigation)
                    .WithMany(p => p.Dealer)
                    .HasForeignKey(d => d.TumbolCode)
                    .HasConstraintName("FK_Dealer_Tumbol");
            });

            modelBuilder.Entity<Enrollment>(entity =>
            {
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

                entity.Property(e => e.Tel)
                    .IsRequired()
                    .HasMaxLength(50);
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
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.BrandId)
                    .HasConstraintName("FK_Product_Brand");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Staff");

                entity.HasOne(d => d.ProductType)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.ProductTypeId)
                    .HasConstraintName("FK_Product_ProductType");
            });

            modelBuilder.Entity<ProductAttachment>(entity =>
            {
                entity.Property(e => e.Extension)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Path).IsRequired();

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductAttachment)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductAttachment_Product");
            });

            modelBuilder.Entity<ProductType>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.ProductType)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductType_Brand");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.ProductType)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductType_Staff");
            });

            modelBuilder.Entity<Promotion>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Brand)
                    .WithMany(p => p.Promotion)
                    .HasForeignKey(d => d.BrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Promotion_Brand");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Promotion)
                    .HasForeignKey(d => d.CreatedBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Promotion_Staff");

                entity.HasOne(d => d.PromotionType)
                    .WithMany(p => p.Promotion)
                    .HasForeignKey(d => d.PromotionTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Promotion_PromotionType");
            });

            modelBuilder.Entity<PromotionType>(entity =>
            {
                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
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
                entity.Property(e => e.Code).HasMaxLength(10);

                entity.Property(e => e.ScanDate).HasColumnType("datetime");

                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Consumer)
                    .WithMany(p => p.QrCode)
                    .HasForeignKey(d => d.ConsumerId)
                    .HasConstraintName("FK_QrCode_Consumer");
            });

            modelBuilder.Entity<Role>(entity =>
            {
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
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.ResetPasswordToken).HasMaxLength(50);

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

            modelBuilder.Entity<Theme>(entity =>
            {
                entity.Property(e => e.HtmlCode).IsRequired();

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ThemeName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<ThemeConfig>(entity =>
            {
                entity.HasKey(e => new { e.BrandId, e.ThemeId });

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Facebook).HasMaxLength(100);

                entity.Property(e => e.Line).HasMaxLength(100);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.TextValue01).HasMaxLength(50);

                entity.Property(e => e.TextValue02).HasMaxLength(50);

                entity.Property(e => e.TextValue03).HasMaxLength(50);

                entity.Property(e => e.Twitter).HasMaxLength(100);

                entity.Property(e => e.WebSite).HasMaxLength(100);

                entity.HasOne(d => d.Theme)
                    .WithMany(p => p.ThemeConfig)
                    .HasForeignKey(d => d.ThemeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ThemeConfig_Theme");
            });

            modelBuilder.Entity<Tracking>(entity =>
            {
                entity.HasKey(e => new { e.PromitionId, e.ConsumerId, e.SendBy });

                entity.Property(e => e.SendDate).HasColumnType("datetime");

                entity.Property(e => e.SendType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.HasOne(d => d.Promition)
                    .WithMany(p => p.Tracking)
                    .HasForeignKey(d => d.PromitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tracking_Consumer");

                entity.HasOne(d => d.PromitionNavigation)
                    .WithMany(p => p.Tracking)
                    .HasForeignKey(d => d.PromitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tracking_Promotion");

                entity.HasOne(d => d.SendByNavigation)
                    .WithMany(p => p.Tracking)
                    .HasForeignKey(d => d.SendBy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tracking_Staff");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Latitude).HasMaxLength(50);

                entity.Property(e => e.Location).HasMaxLength(255);

                entity.Property(e => e.Longitude).HasMaxLength(50);

                entity.Property(e => e.ResponseMessage).IsRequired();

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
