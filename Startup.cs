using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using CSP_Redemption_WebApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using CSP_Redemption_WebApi.Repositories;
using CSP_Redemption_WebApi.Entities.DBContext;

namespace CSP_Redemption_WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Culture Info
            var cultureInfo = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            //Context
            services.AddDbContext<CSP_RedemptionContext>();

            // Services
            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IFunctionService, FunctionService>();
            services.AddScoped<IConsumerService, ConsumerService>();
            services.AddScoped<ICampaignTypeService, CampaignTypeService>();
            services.AddScoped<ICampaignService, CampaignService>();
            services.AddScoped<IDealerService, DealerService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IQrCodeService, QrCodeService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IChartService, ChartService>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<IEnrollmentService, EnrollmentService>();
            services.AddScoped<IProductTypeService, ProductTypeService>();
            services.AddScoped<IPromotionService, PromotionService>();
            services.AddScoped<IPromotionTypeService, PromotionTypeService>();

            // Repositories
            services.AddScoped(typeof(IStaffRepository), typeof(StaffRepository));
            services.AddScoped(typeof(IFunctionRepository), typeof(FunctionRepository));
            services.AddScoped(typeof(IRoleFunctionRepository), typeof(RoleFunctionRepository));
            services.AddScoped(typeof(IZoneRepository), typeof(ZoneRepository));
            services.AddScoped(typeof(IProvinceRepository), typeof(ProvinceRepository));
            services.AddScoped(typeof(IAmphurRepository), typeof(AmphurRepository));
            services.AddScoped(typeof(ITumbolRepository), typeof(TumbolRepository));
            services.AddScoped(typeof(IConsumerRepository), typeof(ConsumerRepository));
            services.AddScoped(typeof(ICampaignRepository), typeof(CampaignRepository));
            services.AddScoped(typeof(ITransactionRepository), typeof(TransactionRepository));
            services.AddScoped(typeof(IQrCodeRepository), typeof(QrCodeRepository));
            services.AddScoped(typeof(ICampaignTypeRepository), typeof(CampaignTypeRepository));
            services.AddScoped(typeof(ICampaignRepository), typeof(CampaignRepository));
            services.AddScoped(typeof(IDealerRepository), typeof(DealerRepository));
            services.AddScoped(typeof(ProductRepository), typeof(ProductRepository));
            services.AddScoped(typeof(IRoleRepository), typeof(RoleRepository));
            services.AddScoped(typeof(IBrandRepository), typeof(BrandRepository));
            services.AddScoped(typeof(IEnrollmentRepository), typeof(EnrollmentRepository));
            services.AddScoped(typeof(IProductTypeRepository), typeof(ProductTypeRepository));
            services.AddScoped(typeof(IPromotionRepository), typeof(PromotionRepository));
            services.AddScoped(typeof(IPromotionTypeRepository), typeof(PromotionTypeRepository));

            // JWT Authentication
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = false;
                config.SaveToken = true;
                config.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidIssuer = Configuration["JWTAuthentication:Issuer"],
                    ValidAudience = Configuration["JWTAuthentication:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWTAuthentication:Key"])),
                    ClockSkew = TimeSpan.Zero,
                };
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Authenticate
            app.UseAuthentication();

            //Add CORS
            app.UseCors(builder =>
            {
                builder
                    .SetPreflightMaxAge(TimeSpan.MaxValue)
                    .AllowAnyMethod()
                    .WithOrigins("http://localhost:4200")
                    .WithOrigins("https://etax.chanwanich.com/csp-redemption-ui/")
                    .AllowCredentials()
                    .AllowAnyHeader();
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
