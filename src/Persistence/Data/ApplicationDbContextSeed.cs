using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Persistence.Data
{
    public class ApplicationDbContextSeed
    {
        public static async Task SeedData(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var users = new List<AppUser>
                {
                    new AppUser
                    {
                        Id = "a",
                        DisplayName = "Bob",
                        UserName = "bob",
                        Email = "bob@test.com"
                    },
                    new AppUser
                    {
                        Id = "b",
                        DisplayName = "Jane",
                        UserName = "jane",
                        Email = "jane@test.com"
                    },
                    new AppUser
                    {
                        Id = "c",
                        DisplayName = "Tom",
                        UserName = "tom",
                        Email = "tom@test.com"
                    },
                    new AppUser
                    {
                        Id = "d",
                        DisplayName = "Ania",
                        UserName = "an1122",
                        Email = "ania@test.com"
                    },
                    new AppUser
                    {
                        Id = "e",
                        DisplayName = "Mark",
                        UserName = "ma44",
                        Email = "mark@test.com"
                    },
                    new AppUser
                    {
                        Id = "f",
                        DisplayName = "Tom",
                        UserName = "tomek1167",
                        Email = "tomek@test.com"
                    },
                    new AppUser
                    {
                        Id = "g",
                        DisplayName = "Jan",
                        UserName = "tom11",
                        Email = "jan@test.com"
                    },
                    new AppUser
                    {
                        Id = "h",
                        DisplayName = "Kasia",
                        UserName = "kasia1167",
                        Email = "kasia@test.com"
                    },
                     new AppUser
                    {
                        Id = "i",
                        DisplayName = "Mark",
                        UserName = "marek1167",
                        Email = "marek@test.com"
                    },
                };

                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "Pa$$w0rd");
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
