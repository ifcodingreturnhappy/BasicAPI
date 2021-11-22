using API.Services;
using DataLayer;
using DataLayer.Abstractions;
using DataLayer.LiteDB;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace API
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
            services.AddCors();
            services.AddControllers();

            InitializeDependencyInjection(services);

            // Initialize token secret
            var settings = AuthSettings.GetSettings(Configuration);
            services.AddSingleton(settings);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(settings.AUTH_SECRET)),
                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void InitializeDependencyInjection(IServiceCollection services)
        {
            services.AddSingleton<ITokenService, TokenService>();

            services.AddTransient<IDBWriter>( _ => new LiteDBWrite(Configuration.GetConnectionString("DEV"))); // TODO: Change hard coded to dynamic according to release type
            services.AddTransient<IDBReader>( _ => new LiteDBRead(Configuration.GetConnectionString("DEV"))); // TODO: Change hard coded to dynamic according to release type
            services.AddTransient<IDBDeleter>( _ => new LiteDBDelete(Configuration.GetConnectionString("DEV"))); // TODO: Change hard coded to dynamic according to release type
            services.AddTransient<IPasswordHasherService, HMACHashingService>();
        }
    }
}
