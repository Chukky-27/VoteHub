using Microsoft.EntityFrameworkCore;
using VotingAppApi.Models;

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
