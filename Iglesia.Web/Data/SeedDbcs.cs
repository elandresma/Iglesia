using Iglesia.Common.Entities;
using Iglesia.Common.Enum;
using Iglesia.Web.Data.Entities;
using Iglesia.Web.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Iglesia.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;

        public SeedDb(DataContext context, IUserHelper userHelper, IBlobHelper blobHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckRegionsAsync();
            await CheckProfessionsAsync();
            await CheckRolesAsync();
            await CheckUserAsync("1010", "Andres", "Marin", "andres521marin@gmail.com", "301 734 8518", "Calle Luna Calle Sol", UserType.Admin, "Pepe.png", new Church(), new Profession());
            await CheckUsersAsync();
        }

        private async Task CheckProfessionsAsync()
        {
            if (!_context.Professions.Any())
            {
                _context.Professions.Add(new Profession
                {
                    Name = "Lawer"
                });
                _context.Professions.Add(new Profession
                {
                    Name = "Engineer"
                });
                _context.Professions.Add(new Profession
                {
                    Name = "Priest"
                });
                _context.Professions.Add(new Profession
                {
                    Name = "Doctor"
                });
            }
            await _context.SaveChangesAsync();
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
                            new Church { Name = "Church 2" }
                        }
                    },
                    new District
                    {
                        Name = "District 2",
                        Churches = new List<Church>
                        {
                            new Church { Name = "Church 3" },
                             new Church { Name = "Church 4" },
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
                            new Church { Name = "Church 5" },
                            new Church { Name = "Church 6" }
                        }
                    },
                    new District
                    {
                        Name = "District 5",
                        Churches = new List<Church>
                        {
                            new Church { Name = "Church 7" },
                            new Church { Name = "Church 8" }
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
            await _userHelper.CheckRoleAsync(UserType.Teacher.ToString());
        }
        private async Task CheckUsersAsync()
        {
            List<Photo> photos = LoadPhotos();
            int i = 0;
            foreach (Photo photo in photos)
            {
                i++;
                Profession profession = await _context.Professions.FirstOrDefaultAsync(c => c.Name == photo.Profession);
                Church church = await _context.Churches.FirstOrDefaultAsync(c => c.Name == photo.Church);
                await CheckUserAsync($"1002158{i}", photo.Firstname, photo.Lastname, $"user{i}@yopmail.com", $"350 634 274{i}", $"Calle Luna Calle{i} Sol", UserType.User, photo.Image, church, profession);
            }

            List<Photo> photosTeacher = LoadTeacher();
            int j = 0;
            foreach (Photo photo in photosTeacher)
            {
                j++;
                Profession profession = await _context.Professions.FirstOrDefaultAsync(c => c.Name == photo.Profession);
                Church church = await _context.Churches.FirstOrDefaultAsync(c => c.Name == photo.Church);
                await CheckUserAsync($"1002158{j}", photo.Firstname, photo.Lastname, $"Teacher{j}@yopmail.com", $"350 634 274{j}", $"Calle Profe Calle{j} Sol", UserType.Teacher, photo.Image, church, profession);
            }
        }

        private List<Photo> LoadTeacher()
        {
            return new List<Photo>
            {
                new Photo { Firstname = "Eduard", Lastname = "Vergara", Image = "Eduard.jpg", Profession= "Lawer", Church="Church 1"},
                new Photo { Firstname = "Hernesto", Lastname = "Lopez", Image = "Hernesto.jpg", Profession= "Priest", Church="Church 2"},
                new Photo { Firstname = "Mariana", Lastname = "Cardona", Image = "Mariana.jpg", Profession= "Doctor", Church="Church 3" },
                new Photo { Firstname = "Mark", Lastname = "Echavarria", Image = "Mark.jpg", Profession = "Priest", Church = "Church 4" },
                new Photo { Firstname = "Nataly", Lastname = "Sanchez", Image = "Nataly.jpg", Profession= "Lawer", Church="Church 5"},
                new Photo { Firstname = "Sofia", Lastname = "Medez", Image = "Sofia.jpg" , Profession= "Doctor", Church="Church 6"},
                new Photo { Firstname = "Sonia", Lastname = "Smith", Image = "Sonia.jpg" , Profession= "Lawer", Church="Church 7"},
                new Photo { Firstname = "Valentina", Lastname = "Rogers", Image = "Valentina.jpg" , Profession= "Doctor", Church="Church 8"},
            };
        }  

        private List<Photo> LoadPhotos()
        {
            return new List<Photo>
            {
                new Photo { Firstname = "Adala", Lastname = "Samir", Image = "Adala.jpg", Profession= "Lawer", Church="Church 1"},
                new Photo { Firstname = "Amalia", Lastname = "Lopez", Image = "Amalia.jpg", Profession= "Priest", Church="Church 1"},
                new Photo { Firstname = "Camila", Lastname = "Cardona", Image = "Camila.jpg", Profession= "Doctor", Church="Church 1" },
                new Photo { Firstname = "Carolina", Lastname = "Echavarria", Image = "Carolina.jpg", Profession = "Priest", Church = "Church 1" },
                new Photo { Firstname = "Claudia", Lastname = "Sanchez", Image = "Claudia.jpg", Profession= "Lawer", Church="Church 1"},
                new Photo { Firstname = "Gilberto", Lastname = "Medez", Image = "Gilberto.jpg" , Profession= "Doctor", Church="Church 2"},
                new Photo { Firstname = "Jhon", Lastname = "Smith", Image = "Jhon.jpg" , Profession= "Lawer", Church="Church 2"},
                new Photo { Firstname = "Ken", Lastname = "Rogers", Image = "Ken.jpg" , Profession= "Doctor", Church="Church 2"},
                new Photo { Firstname = "Laura", Lastname = "Zuluaga", Image = "Laura.jpg", Profession= "Lawer", Church="Church 2" },
                new Photo { Firstname = "Luisa", Lastname = "Zapata", Image = "Luisa.jpg" , Profession= "Engineer", Church="Church 2"},
                new Photo { Firstname = "Manuel", Lastname = "Rodriguez", Image = "Manuel.jpg" , Profession= "Engineer", Church="Church 3"},
                new Photo { Firstname = "Manuela", Lastname = "Ateortua", Image = "Manuela.jpg" , Profession= "Priest", Church="Church 3"},
                new Photo { Firstname = "Mario", Lastname = "Bedoya", Image = "Mario.jpg" , Profession= "Engineer", Church="Church 3"},
                new Photo { Firstname = "Monica", Lastname = "Cano", Image = "Monica.jpg" , Profession= "Doctor", Church="Church 3"},
                new Photo { Firstname = "Pedro", Lastname = "Correa", Image = "Pedro.jpg" , Profession= "Engineer", Church="Church 3"},
                new Photo { Firstname = "Penelope", Lastname = "Arias", Image = "Penelope.jpg", Profession= "Priest", Church="Church 4" },
                new Photo { Firstname = "Pepe", Lastname = "Lopez", Image = "Pepe.png" , Profession= "Engineer", Church="Church 4" },
                new Photo { Firstname = "Raul", Lastname = "Matinez", Image = "Raul.jpg" , Profession= "Priest", Church="Church 4" },
                new Photo { Firstname = "Roberto", Lastname = "Rivas", Image = "Roberto.jpg", Profession= "Engineer", Church="Church 4"  },
                new Photo { Firstname = "Rosa", Lastname = "Velasquez", Image = "Rosa.jpg" , Profession= "Lawer", Church="Church 4" },
                new Photo { Firstname = "Rosario", Lastname = "Sandoval", Image = "Rosario.jpg"  , Profession= "Lawer", Church="Church 5" },
                new Photo { Firstname = "Sandra", Lastname = "Machado", Image = "Sandra.jpg" , Profession= "Doctor", Church="Church 5"},
                new Photo { Firstname = "Sandro", Lastname = "Ruiz", Image = "Sandro.jpg" , Profession= "Lawer", Church="Church 5"},
                new Photo { Firstname = "Teresa", Lastname = "Santamaria", Image = "Teresa.jpg", Profession= "Doctor", Church="Church 5" },
                new Photo { Firstname = "Ana", Lastname = "Goez", Image = "Ana.png", Profession= "Priest", Church="Church 5" },
                new Photo { Firstname = "Camilo", Lastname = "Marin", Image = "Camilo.jpg", Profession= "Engineer", Church="Church 6" },
                new Photo { Firstname = "David", Lastname = "Bolivar", Image = "David.jpg", Profession= "Doctor", Church="Church 6" },
                new Photo { Firstname = "Elena", Lastname = "Sanchez", Image = "Elena.jpg", Profession= "Lawer", Church="Church 6" },
                new Photo { Firstname = "Veronica", Lastname = "Rodriguez", Image = "Veronica.jpg", Profession= "Lawer", Church="Church 6" },
                new Photo { Firstname = "Jose", Lastname = "Velez", Image = "Jose.jpg", Profession= "Doctor", Church="Church 6" },
                new Photo { Firstname = "Juan", Lastname = "Hoyos", Image = "Juan.jpg", Profession= "Engineer", Church="Church 7" },
                new Photo { Firstname = "Cecilia", Lastname = "Padilla", Image = "Cecilia.jpg", Profession= "Engineer", Church="Church 7" },
                new Photo { Firstname = "Benito", Lastname = "Rodriguez", Image = "Benito.jpg", Profession= "Priest", Church="Church 7" },
                new Photo { Firstname = "Erika", Lastname = "Lopez", Image = "Erika.jpg", Profession= "Lawer", Church="Church 7" },
                new Photo { Firstname = "Esperanza", Lastname = "Gomez", Image = "Esperanza.jpg", Profession= "Doctor", Church="Church 7" },
                new Photo { Firstname = "Fernanda", Lastname = "Martinez", Image = "Fernanda.jpg", Profession= "Lawer", Church="Church 8" },
                new Photo { Firstname = "Ramon", Lastname = "Cano", Image = "Ramon.jpg", Profession= "Engineer", Church="Church 8" },
                new Photo { Firstname = "Raquel", Lastname = "Berrio", Image = "Raquel.jpg", Profession= "Lawer", Church="Church 8" },
                new Photo { Firstname = "Renata", Lastname = "Correa", Image = "Renata.jpg", Profession= "Engineer", Church="Church 8" },
                new Photo { Firstname = "Renato", Lastname = "Rivas", Image = "Renato.jpg", Profession= "Doctor", Church="Church 8" }
                
            };
        }
        private async Task<User> CheckUserAsync(
            string document,
            string firstName,
            string lastName,
            string email,
            string phone,
            string address,
            UserType userType,
            string image,
            Church church,
            Profession profession)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\images\\Users", image);
                Guid imageId = await _blobHelper.UploadBlobAsync(path, "users");

                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    ImageId = imageId,
                    UserType = userType,
                };

                if (!userType.ToString().Equals("Admin"))
                {
                    user.Church = church;
                    user.Profession = profession;

                }

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
                string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);

            }

            return user;
        }

    }
    public class Photo
    {
        public string Firstname { get; set; }

        public string Lastname { get; set; }


        public string Image { get; set; }
        public string Profession { get; set; }

        public string Church { get; set; }
    }

}
