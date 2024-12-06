using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VoteHub.Persistance.IRepositories;
using VoteHub.Persistance.Repositories;
using VotingAppApi.Data;

namespace VoteHub.Persistance.Extensions
{
    public static class DIServiceExtension
    {
        public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<VotingAppDbContext>(opt => 
            opt.UseSqlServer(configuration.GetConnectionString("VoteHubConn")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IVotingEventRepository, VotingEventRepository>();

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        }
    }

}
