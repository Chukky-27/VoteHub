using Microsoft.EntityFrameworkCore;
namespace VotingAppApi.Data
{
    public class VotingAppDbContext : DbContext 
    {
        public VotingAppDbContext(DbContextOptions<VotingAppDbContext> options) 
            : base(options)
        {

        }
    }
      
}
