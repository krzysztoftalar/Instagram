using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistence.Data
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedData(ApplicationDbContext context, UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                foreach (var user in GetPreconfiguredUsers())
                {
                    await userManager.CreateAsync(user, "Pa$$w0rd");
                }

                await context.SaveChangesAsync();

                await context.Followings.AddRangeAsync(GetPreconfiguredFollowers());

                await context.SaveChangesAsync();
            }
        }

        private static IEnumerable<AppUser> GetPreconfiguredUsers()
        {
            return new List<AppUser>
            {
                new AppUser
                {
                    Id = "a",
                    DisplayName = "Bob",
                    UserName = "bob",
                    Email = "bob@test.com",
                },
                new AppUser
                {
                    Id = "b",
                    DisplayName = "Agata",
                    UserName = "agata",
                    Email = "agata@test.com"
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
                new AppUser
                {
                    Id = "j",
                    DisplayName = "Monika",
                    UserName = "monia",
                    Email = "monia@test.com"
                },
                new AppUser
                {
                    Id = "k",
                    DisplayName = "Jurek",
                    UserName = "jurek",
                    Email = "jurek@test.com"
                },
                new AppUser
                {
                    Id = "l",
                    DisplayName = "Olek",
                    UserName = "olek",
                    Email = "olek@test.com"
                },
                new AppUser
                {
                    Id = "m",
                    DisplayName = "Barbara",
                    UserName = "barbara",
                    Email = "barbara@test.com"
                },
                new AppUser
                {
                    Id = "n",
                    DisplayName = "Kazimierz",
                    UserName = "kazek",
                    Email = "kazek@test.com"
                },
                new AppUser
                {
                    Id = "o",
                    DisplayName = "Olek",
                    UserName = "olek22",
                    Email = "olek22@test.com"
                },
                new AppUser
                {
                    Id = "p",
                    DisplayName = "Tom Kowalski",
                    UserName = "tom1122",
                    Email = "tom1122@test.com"
                },
                new AppUser
                {
                    Id = "q",
                    DisplayName = "Filip",
                    UserName = "filip",
                    Email = "filip@test.com"
                },
                new AppUser
                {
                    Id = "r",
                    DisplayName = "Edward",
                    UserName = "edward",
                    Email = "edward@test.com"
                },
                new AppUser
                {
                    Id = "s",
                    DisplayName = "Maja",
                    UserName = "maja",
                    Email = "maja@test.com"
                },
                new AppUser
                {
                    Id = "t",
                    DisplayName = "Piotr",
                    UserName = "piotr",
                    Email = "piotr@test.com"
                },
                new AppUser
                {
                    Id = "u",
                    DisplayName = "Alan",
                    UserName = "alan",
                    Email = "alan@test.com"
                },
                new AppUser
                {
                    Id = "v",
                    DisplayName = "Justyna",
                    UserName = "justyna",
                    Email = "justyna@test.com"
                },
            };
        }

        private static IEnumerable<UserFollowing> GetPreconfiguredFollowers()
        {
            return new List<UserFollowing>
            {
                new UserFollowing{ObserverId = "b", TargetId = "a"},
                new UserFollowing{ObserverId = "c", TargetId = "a"},
                new UserFollowing{ObserverId = "d", TargetId = "a"},
                new UserFollowing{ObserverId = "e", TargetId = "a"},
                new UserFollowing{ObserverId = "f", TargetId = "a"},
                new UserFollowing{ObserverId = "g", TargetId = "a"},
                new UserFollowing{ObserverId = "h", TargetId = "a"},
                new UserFollowing{ObserverId = "i", TargetId = "a"},
                new UserFollowing{ObserverId = "j", TargetId = "a"},
                new UserFollowing{ObserverId = "k", TargetId = "a"},
                new UserFollowing{ObserverId = "l", TargetId = "a"},
                new UserFollowing{ObserverId = "m", TargetId = "a"},
                new UserFollowing{ObserverId = "n", TargetId = "a"},
                new UserFollowing{ObserverId = "o", TargetId = "a"},
                new UserFollowing{ObserverId = "p", TargetId = "a"},
                new UserFollowing{ObserverId = "q", TargetId = "a"},
                new UserFollowing{ObserverId = "r", TargetId = "a"},
                new UserFollowing{ObserverId = "s", TargetId = "a"},
                new UserFollowing{ObserverId = "t", TargetId = "a"},
                new UserFollowing{ObserverId = "u", TargetId = "a"},
                new UserFollowing{ObserverId = "v", TargetId = "a"},
            };
        }
    }
}


