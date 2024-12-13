using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VoteHub.Persistance.Repositories.Implementation;
using VoteHub.Persistance.Repositories.Interfaces;
using VoteHub.Persistance.Services.Implementation;
using VoteHub.Persistance.Services.Interfaces;
using VotingAppApi.Data;
using VotingAppApi.Models;

namespace VoteHub.Persistance.Extensions
{
    public static class DIServiceExtension
    {
        public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<VotingAppDbContext>(opt => 
            opt.UseSqlServer(configuration.GetConnectionString("VoteHubConn")));

            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<VotingAppDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                    };
                });

            services.AddAuthorization();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IVotingEventRepository, VotingEventRepository>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped<IDistributedCacheService, DistributedCacheService>();

            services.AddMemoryCache();

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost:6379"; // Replace with your Redis server configuration
                options.InstanceName = "VotingApp_";
            });

            services.AddScoped<IRoleSeeder, RoleSeeder>();

            //services.AddDistributedRedisCache(options =>
            //{
            //    options.Configuration = "localhost:6379";
            //    options.InstanceName = "Session:";
            //});

            //services.AddSession(options =>
            //{
            //    options.IdleTimeout = TimeSpan.FromMinutes(20);
            //    options.Cookie.HttpOnly = true;
            //    options.Cookie.IsEssential = true;
            //});



        }
    }

}
