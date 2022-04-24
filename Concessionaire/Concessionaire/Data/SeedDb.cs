using Concessionaire.Data.Entities;
using Concessionaire.Enums;
using Concessionaire.Helpers;

namespace Concessionaire.Data
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
            await CheckBrandsAsync();
            await CheckCountriesAsync();
            await CheckRolesAsync();
            await CheckVehicleTypesAsync();
            await CheckUserAsync("1010", "Brad", "Pitt", "brad@yopmail.com", "300 422 4861", "Cra 50 # 133 sur 80", "Brad.jpg", UserType.Admin);
            await CheckUserAsync("2020", "Angelina", "Jolie", "angelina@yopmail.com", "311 515 3245", "Calle 12 # 40 - 20", "Angelina.jpg", UserType.User);
        }

        private async Task<User> CheckUserAsync(
            string document,
            string firstName,
            string lastName,
            string email,
            string phone,
            string address,
            string image,
            UserType userType)
        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                Guid imageId = await _blobHelper.UploadBlobAsync($"{Environment.CurrentDirectory}\\wwwroot\\images\\users\\{image}", "users");
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    City = _context.Cities.FirstOrDefault(),
                    UserType = userType,
                    ImageId = imageId,
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());

                string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }

            return user;
        }


        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                _context.Countries.Add(new Country
                {
                    Name = "Colombia",
                    States = new List<State>()
                    {
                        new State()
                        {
                            Name = "Antioquia",
                            Cities = new List<City>() {
                                new City() { Name = "Medellín" },
                                new City() { Name = "Itagüí" },
                                new City() { Name = "Envigado" },
                                new City() { Name = "Bello" },
                                new City() { Name = "Sabaneta" },
                                new City() { Name = "La Ceja" },
                                new City() { Name = "La Union" },
                                new City() { Name = "La Estrella" },
                                new City() { Name = "Copacabana" },
                                new City() { Name = "Caldas" },
                            }
                        },
                        new State()
                        {
                            Name = "Bogotá",
                            Cities = new List<City>() {
                                new City() { Name = "Usaquen" },
                                new City() { Name = "Champinero" },
                                new City() { Name = "Santa fe" },
                                new City() { Name = "Usme" },
                                new City() { Name = "Bosa" },
                            }
                        },
                        new State()
                        {
                            Name = "Valle",
                            Cities = new List<City>() {
                                new City() { Name = "Santiago de Cali" },
                                new City() { Name = "Jumbo" },
                                new City() { Name = "Jamundí" },
                                new City() { Name = "Chipichape" },
                                new City() { Name = "Buenaventura" },
                                new City() { Name = "Cartago" },
                                new City() { Name = "Buga" },
                                new City() { Name = "Palmira" },
                            }
                        },
                        new State()
                        {
                            Name = "Santander",
                            Cities = new List<City>() {
                                new City() { Name = "Bucaramanga" },
                                new City() { Name = "Málaga" },
                                new City() { Name = "Barrancabermeja" },
                                new City() { Name = "Rionegro" },
                                new City() { Name = "Barichara" },
                                new City() { Name = "Zapatoca" },
                            }
                        },
                        new State()
                        {
                            Name = "Atlantico",
                            Cities = new List<City>() {
                                new City() { Name = "Barranquilla" },
                                new City() { Name = "Puerto Colombia" },
                                new City() { Name = "Soledad" },
                                new City() { Name = "Malambo" },
                                new City() { Name = "Candelaria" },
                                new City() { Name = "Sabanalarga" },
                            }
                        },
                    }
                });
                _context.Countries.Add(new Country
                {
                    Name = "Estados Unidos",
                    States = new List<State>()
                    {
                        new State()
                        {
                            Name = "Florida",
                            Cities = new List<City>() {
                                new City() { Name = "Orlando" },
                                new City() { Name = "Miami" },
                                new City() { Name = "Tampa" },
                                new City() { Name = "Fort Lauderdale" },
                                new City() { Name = "Key West" },
                            }
                        },
                        new State()
                        {
                            Name = "Texas",
                            Cities = new List<City>() {
                                new City() { Name = "Houston" },
                                new City() { Name = "San Antonio" },
                                new City() { Name = "Dallas" },
                                new City() { Name = "Austin" },
                                new City() { Name = "El Paso" },
                            }
                        },
                        new State()
                        {
                            Name = "California",
                            Cities = new List<City>() {
                                new City() { Name = "Los Angeles" },
                                new City() { Name = "San Francisco" },
                                new City() { Name = "San Diego" },
                                new City() { Name = "San Bruno" },
                                new City() { Name = "Sacramento" },
                                new City() { Name = "Fresno" },

                            }
                        },
                        new State()
                        {
                            Name = "Massachusetts",
                            Cities = new List<City>() {
                                new City() { Name = "Boston" },
                                new City() { Name = "Springfield" },
                                new City() { Name = "Newton" },
                                new City() { Name = "Revere" },
                                new City() { Name = "Cambridge" },
                                new City() { Name = "Braintree" },

                            }
                        },
                    }
                });
                _context.Countries.Add(new Country
                {
                    Name = "Ecuador",
                    States = new List<State>()
                    {
                        new State()
                        {
                            Name = "Pichincha",
                            Cities = new List<City>() {
                                new City() { Name = "Quito" },
                                new City() { Name = "Cayambe" },
                                new City() { Name = "Mejía" },
                                new City() { Name = "Rumiñahui" },
                                new City() { Name = "San Miguel de Los Bancos" },
                            }
                        },
                        new State()
                        {
                            Name = "Esmeraldas",
                            Cities = new List<City>() {
                                new City() { Name = "Esmeraldas" },
                                new City() { Name = "Atacames" },
                                new City() { Name = "Eloy Alfaro" },
                                new City() { Name = "Muisne" },
                                new City() { Name = "San Lorenzo" },
                            }
                        },
                        new State()
                        {
                            Name = "Carchi",
                            Cities = new List<City>() {
                                new City() { Name = "Tulcán" },
                                new City() { Name = "Bolívar" },
                                new City() { Name = "Espejo" },
                                new City() { Name = "Mira" },
                                new City() { Name = "Montúfar" },
                            }
                        },
                    }
                });

            }

            await _context.SaveChangesAsync();
        }

        private async Task CheckVehicleTypesAsync()
        {
            if (!_context.VehicleTypes.Any())
            {
                _context.VehicleTypes.Add(new VehicleType { Name = "Carro" });
                _context.VehicleTypes.Add(new VehicleType { Name = "Moto" });
                _context.VehicleTypes.Add(new VehicleType { Name = "Camión" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckBrandsAsync()
        {
            if (!_context.Brands.Any())
            {
                _context.Brands.Add(new Brand { Name = "Mazda" });
                _context.Brands.Add(new Brand { Name = "Renault" });
                _context.Brands.Add(new Brand { Name = "Chevrolet" });
                _context.Brands.Add(new Brand { Name = "Suzuki" });
                _context.Brands.Add(new Brand { Name = "AKT" });
                _context.Brands.Add(new Brand { Name = "TVS" });
                _context.Brands.Add(new Brand { Name = "Foton" });
                _context.Brands.Add(new Brand { Name = "Ford" });
                _context.Brands.Add(new Brand { Name = "Audi" });
                _context.Brands.Add(new Brand { Name = "BMW" });
                _context.Brands.Add(new Brand { Name = "Mercedes Benz" });
                await _context.SaveChangesAsync();
            }
        }
    }
}
