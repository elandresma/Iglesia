using Iglesia.Common.Entities;
using Iglesia.Common.Enum;
using Iglesia.Web.Data.Entities;
using Iglesia.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iglesia.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckRegionsAsync();
            await CheckRolesAsync();
            await CheckUserAsync("1010", "Andres", "Marin", "andres521marin@gmail.com", "301 734 8518", "Calle Luna Calle Sol", UserType.Admin);

        }

        private async Task CheckRegionsAsync()
        {
            if (!_context.Regions.Any())
            {
                _context.Regions.Add(new Region
                {
                    Name = "Region 1",
                    Districts = new List<District>
                {
                    new District
                    {
                        Name = "District 1",
                        Churches = new List<Church>
                        {
                            new Church { Name = "Church 1" },
                            new Church { Name = "Church 2" },
                            new Church { Name = "Church 3" }
                        }
                    },
                    new District
                    {
                        Name = "District 2",
                        Churches = new List<Church>
                        {
                            new Church { Name = "Church 4" },
                        }
                    },
                    new District
                    {
                        Name = "District 3",
                        Churches = new List<Church>
                        {
                            new Church { Name = "Church 5" },
                            new Church { Name = "Church 6" },
                            new Church { Name = "Church 7" }
                        }
                    }
                }
                });
                _context.Regions.Add(new Region
                {
                    Name = "Region 2",
                    Districts = new List<District>
                {
                    new District
                    {
                        Name = "District 4",
                        Churches = new List<Church>
                        {
                            new Church { Name = "Church 8" },
                            new Church { Name = "Church 9" },
                            new Church { Name = "Church 10" }
                        }
                    },
                    new District
                    {
                        Name = "District 5",
                        Churches = new List<Church>
                        {
                            new Church { Name = "Church 11" },
                            new Church { Name = "Church 12" },
                            new Church { Name = "Church 13" }
                        }
                    }
                }
                });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task<User> CheckUserAsync(
            string document,
            string firstName,
            string lastName,
            string email,
            string phone,
            string address,
            UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    Church = _context.Churches.FirstOrDefault(),
                    Profession = _context.Professions.FirstOrDefault(),
                    UserType = userType
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
            }

            return user;
        }

    }

}
