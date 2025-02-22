using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoteHub.Domain.Entities;
using VoteHub.Domain.Enums;
using VotingAppApi.Data;
using VotingAppApi.Models;

namespace VoteHub.Persistance
{
    // Seed data method
    public class DataSeeder
    {
        private readonly VotingAppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DataSeeder(VotingAppDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            await SeedRolesAsync();
            await SeedUsersAsync();
            await SeedVotingEventsAsync();
            await SeedCandidatesAsync();
        }

        private async Task SeedRolesAsync()
        {
            var roles = new List<string> { "Admin", "Voter" };

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private async Task SeedUsersAsync()
        {
            var adminEmail = "admin@example.com";
            var voterEmail = "voter@example.com";

            if (await _userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new AppUser
                {
                    Email = adminEmail,
                    UserName = adminEmail,
                    FirstName = "Admin",
                    LastName = "User",
                    Role = UserRole.Admin
                };

                await _userManager.CreateAsync(adminUser, "Admin@123");
                await _userManager.AddToRoleAsync(adminUser, "Admin");
            }

            if (await _userManager.FindByEmailAsync(voterEmail) == null)
            {
                var voterUser = new AppUser
                {
                    Email = voterEmail,
                    UserName = voterEmail,
                    FirstName = "Voter",
                    LastName = "User",
                    Role = UserRole.Voter
                };

                await _userManager.CreateAsync(voterUser, "Voter@123");
                await _userManager.AddToRoleAsync(voterUser, "Voter");
            }
        }

        private async Task SeedVotingEventsAsync()
        {
            if (!_context.VotingEvents.Any())
            {
                var votingEvents = new List<VotingEvent>
                {
                    new VotingEvent
                    {
                        Name = "Annual Election 2023",
                        Status = VotingStatus.Upcoming,
                        StartDate = new DateTime(2023, 11, 1),
                        EndDate = new DateTime(2023, 11, 5),
                        CreatedBy = "System",
                        CreatedDate = DateTime.UtcNow
                    },
                    new VotingEvent
                    {
                        Name = "Club Officer Election",
                        Status = VotingStatus.Ongoing,
                        StartDate = DateTime.UtcNow.AddDays(-1),
                        EndDate = DateTime.UtcNow.AddDays(1),
                        CreatedBy = "System",
                        CreatedDate = DateTime.UtcNow
                    }
                };

                await _context.VotingEvents.AddRangeAsync(votingEvents);
                await _context.SaveChangesAsync();
            }
        }

        private async Task SeedCandidatesAsync()
        {
            if (!_context.Candidates.Any())
            {
                var candidates = new List<Candidate>
                {
                    new Candidate
                    {
                        Name = "John Doe",
                        Position = CandidatePosition.President,
                        VotingEventId = 1, // Assuming the first voting event has ID 1
                        CandidateProfile = "A visionary leader."
                    },
                    new Candidate
                    {
                        Name = "Jane Smith",
                        Position = CandidatePosition.VicePresident,
                        VotingEventId = 1, // Assuming the first voting event has ID 1
                        CandidateProfile = "An experienced administrator."
                    }
                };

                await _context.Candidates.AddRangeAsync(candidates);
                await _context.SaveChangesAsync();
            }
        }
    }


}
