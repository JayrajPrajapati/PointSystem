using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PointSystem.Helper;
using PointSystem.Repositories.Repository;
using PointSystem.Repositories.Services;
using System.Text;

namespace PointSystem
{
    public class Startup
    {
        #region Variable Declaration
        public IConfiguration configuration { get; }
        #endregion

        #region Constructor
        public Startup(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        #endregion

        #region Public Method
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            #region JWT Token

            var appSettingsSection = configuration.GetSection("app_settings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["app_settings:Issuer"],
                    ValidAudience = configuration["app_settings:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };
            });



            #endregion

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PointSystem", Version = "v1.0" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                 {
                   new OpenApiSecurityScheme
                   {
                     Reference = new OpenApiReference
                     {
                       Type = ReferenceType.SecurityScheme,
                       Id = "Bearer"
                     }
                    },
                    new string[] { }
                  }
                });

            });

            #endregion

            #region Connection String

            //services.AddDbContext<MonsterDbContext>(item => item.UseSqlServer(configuration.GetConnectionString("ConnectionString")));            
            //appSettings.ConnectionString = Convert.ToString(configuration.GetConnectionString("AppDbContext"));
            SqlHelper.ConnectionString = ConfigurationExtensions.GetConnectionString(this.configuration, "AppDbContext");
            #endregion

            services.AddHttpContextAccessor();

            services.AddScoped<ILoginRepository, LoginServices>();
            services.AddScoped<ITransactionRepository, TransactionServices>();
            services.AddScoped<ICompanyRepository, CompanyServices>();

            services.AddMvc().AddNewtonsoftJson();
            services.AddControllers();
            services.AddMvc();

            #region Allow api without Authentication

            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
            });

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAuthentication();

            //if (env.IsDevelopment())
            //{
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PointSystem v1.0"));
            //}

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        #endregion
    }
}