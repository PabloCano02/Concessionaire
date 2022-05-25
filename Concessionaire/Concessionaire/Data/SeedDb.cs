using Concessionaire.Data.Entities;
using Concessionaire.Enums;
using Concessionaire.Helpers;
using Microsoft.EntityFrameworkCore;

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
            await CheckVehiclesAsync();
            await CheckUserAsync("1010", "Brad", "Pitt", "brad@yopmail.com", "300 422 4861", "Cra 50 # 133 sur 80", "Brad.jpg", UserType.Admin);
            await CheckUserAsync("2020", "Angelina", "Jolie", "angelina@yopmail.com", "311 515 3245", "Calle 12 # 40 - 20", "Angelina.jpg", UserType.User);
        }

        private async Task CheckVehiclesAsync()
        {
            if (!_context.Vehicles.Any())
            {
                await AddVehicleAsync("HOX67D", "CR4 125", 2020, "Negro", "Ideal para ir al trabajo", 80000M, true, new List<string>() { "AKTCR4125(1).png", "AKTCR4125(2).png", "AKTCR4125(3).png" }, "AKT", "Moto");
                await AddVehicleAsync("POF95G", "Dynamic 125", 2022, "Negro", "Ideal para recorrer la ciudad", 50000M, true, new List<string>() { "AKTDynamic.png" }, "AKT", "Moto");
                await AddVehicleAsync("GHP78D", "Jet 110", 2022, "Negro", "Ideal para recorrer la ciudad", 50000M, true, new List<string>() { "AKTJet.png" }, "AKT", "Moto");
                await AddVehicleAsync("ERP45D", "NKD 125", 2022, "Negro", "Ideal para realizar el trabajo", 60000M, true, new List<string>() { "AKTNKD125.png" }, "AKT", "Moto");
                await AddVehicleAsync("BNK43D", "Special 110", 2021, "Morado", "Ideal para desplazarte hacia el trabajo", 60000M, true, new List<string>() { "AKTSpecial.png" }, "AKT", "Moto");
                await AddVehicleAsync("TRP63D", "TTR 200", 2022, "Negro", "Ideal para alejarte de la ciudad en la que vives y dirigirte a otras ciudades", 100000M, true, new List<string>() { "AKTTTR200.png" }, "AKT", "Moto");
                await AddVehicleAsync("QWE098", "A4", 2018, "Negro", "Confortable para la familia", 200000M, true, new List<string>() { "AudiA4.png" }, "Audi", "Carro");
                await AddVehicleAsync("HGO764", "Q5", 2019, "Negro", "El vehículo se inspiró en la felicidad familiar", 300000M, true, new List<string>() { "AudiQ5.png" }, "Audi", "Carro");
                await AddVehicleAsync("BNV567", "S8", 2020, "Azul", "El vehículo perfecto para competir", 500000M, true, new List<string>() { "AudiS8.png" }, "Audi", "Carro");
                await AddVehicleAsync("POH026", "TT", 2021, "Azul", "El vehículo perfecto para competir", 500000M, true, new List<string>() { "AudiTT.png" }, "Audi", "Carro");
                await AddVehicleAsync("KJH45F", "F 900", 2021, "Rojo", "Ideal para ir a visitar otras ciudades", 350000M, true, new List<string>() { "BMWF900.png" }, "BMW", "Moto");
                await AddVehicleAsync("KLA45B", "K 1600", 2014, "Rojo", "Ideal para ir a visitar otras ciudades", 150000M, true, new List<string>() { "BMWK1600.png" }, "BMW", "Moto");
                await AddVehicleAsync("IGJ23A", "R 1200", 2021, "Negro", "Ideal para recorrer muchos kilometros de carretera", 380000M, true, new List<string>() { "BMWR1200.png" }, "BMW", "Moto");
                await AddVehicleAsync("KSM456", "Serie 7", 2021, "Gris", "El vehículo perfecto para competir", 300000M, true, new List<string>() { "BMWSerie7.png" }, "BMW", "Carro");
                await AddVehicleAsync("FSD845", "X3", 2021, "Azul", "Este vehículo se diseñó para disfrutar de tus viajes", 600000M, true, new List<string>() { "BMWX3.png" }, "BMW", "Carro");
                await AddVehicleAsync("JKL123", "Z4", 2022, "Rojo", "Este vehículo es ideal para viajar con tu pareja", 500000M, true, new List<string>() { "BMWZ4.png" }, "BMW", "Carro");
                await AddVehicleAsync("ASD123", "Aveo", 2022, "Negro", "Confortable para la familia", 180000M, true, new List<string>() { "ChevroletAveo.png" }, "Chevrolet", "Carro");
                await AddVehicleAsync("MNB456", "Camaro", 2022, "Negro", "Ideal para competir", 180000M, true, new List<string>() { "ChevroletCamaro.png" }, "Chevrolet", "Carro");
                await AddVehicleAsync("JAC013", "Captiva", 2021, "Blanco", "Ideal para transitar en la ciudad", 200000M, true, new List<string>() { "ChevroletCaptiva.png" }, "Chevrolet", "Carro");
                await AddVehicleAsync("GFP789", "Cruze", 2022, "Gris", "Ideal para la rutina diaria", 200000M, true, new List<string>() { "ChevroletCruze.png" }, "Chevrolet", "Carro");
                await AddVehicleAsync("UYT123", "Orlando", 2022, "Gris", "Ideal para la rutina diaria", 250000M, true, new List<string>() { "ChevroletOrlando.png" }, "Chevrolet", "Carro");
                await AddVehicleAsync("CVE999", "Spark", 2022, "Naranjado", "Ideal para la rutina diaria", 350000M, true, new List<string>() { "ChevroletSpark.png" }, "Chevrolet", "Carro");
                await AddVehicleAsync("ASP753", "Tracker", 2022, "Rojo", "Ideal para transportarse hasta su propiedad", 450000M, true, new List<string>() { "ChevroletTracker.png" }, "Chevrolet", "Carro");
                await AddVehicleAsync("KLP561", "Trax", 2022, "Azul", "Confortable para la familia", 250000M, true, new List<string>() { "ChevroletTrax.png" }, "Chevrolet", "Carro");
                await AddVehicleAsync("CHA452", "Fiesta", 2020, "Negro", "Este vehículo se diseñó para disfrutar de tus viajes", 150000M, true, new List<string>() { "FordFiesta.png" }, "Ford", "Carro");
                await AddVehicleAsync("ZXC725", "Focus", 2021, "Blanco", "Ideal para la rutina diaria", 200000M, true, new List<string>() { "FordFocus.png" }, "Ford", "Carro");
                await AddVehicleAsync("KFJ785", "Mustang", 2022, "Rojo", "El vehículo perfecto para competir", 900000M, true, new List<string>() { "FordMustang.png" }, "Ford", "Carro");
                await AddVehicleAsync("GFS457", "2", 2022, "Rojo", "Confortable para la familia", 100000M, true, new List<string>() { "Mazda2(1).png", "Mazda2(2).png", "Mazda2(3).png" }, "Mazda", "Carro");
                await AddVehicleAsync("ASB546", "3", 2021, "Blanco", "Ideal para transportarse hasta su propiedad", 150000M, true, new List<string>() { "Mazda3.png" }, "Mazda", "Carro");
                await AddVehicleAsync("BID784", "6", 2021, "Blanco", "Ideal para la rutina diaria", 250000M, true, new List<string>() { "Mazda6.png" }, "Mazda", "Carro");
                await AddVehicleAsync("LAS666", "CX3", 2021, "Rojo", "Este vehículo se diseñó para disfrutar de tus viajes", 300000M, true, new List<string>() { "MazdaCX3.png" }, "Mazda", "Carro");
                await AddVehicleAsync("TAS266", "CX30", 2022, "Gris", "Confortable para la familia", 300000M, true, new List<string>() { "MazdaCX30.png" }, "Mazda", "Carro");
                await AddVehicleAsync("GFS456", "CX5", 2022, "Blanco", "Confortable para la familia", 400000M, true, new List<string>() { "MazdaCX5(1).png", "MazdaCX5(2).png" }, "Mazda", "Carro");
                await AddVehicleAsync("KLP421", "CX9", 2022, "Gris", "Ideal para transportarse hasta su propiedad", 350000M, true, new List<string>() { "MazdaCX9(1).png", "MazdaCX9(2).png" }, "Mazda", "Carro");
                await AddVehicleAsync("DFG839", "MX5", 2021, "Negro", "Ideal para transportarse hasta su propiedad", 350000M, true, new List<string>() { "MazdaMX5.png" }, "Mazda", "Carro");
                await AddVehicleAsync("ASU123", "CLA", 2022, "Rojo", "Confortable para la familia", 350000M, true, new List<string>() { "MercedesClaseCLA.png" }, "Mercedes Benz", "Carro");
                await AddVehicleAsync("IUY456", "G", 2022, "Gris", "Confortable para la familia", 250000M, true, new List<string>() { "MercedesClaseG.png" }, "Mercedes Benz", "Carro");
                await AddVehicleAsync("YUC159", "GLE", 2022, "Rojo", "Confortable para la familia", 450000M, true, new List<string>() { "MercedesClaseGLE.png" }, "Mercedes Benz", "Carro");
                await AddVehicleAsync("UDJ123", "Captur", 2020, "Blanco", "Ideal para transitar en la ciudad", 100000M, true, new List<string>() { "RenaultCaptur.png" }, "Renault", "Carro");
                await AddVehicleAsync("OIU903", "Koleos", 2021, "Negro", "Confortable para la familia", 150000M, true, new List<string>() { "RenaultKoleos.png" }, "Renault", "Carro");
                await AddVehicleAsync("HGN829", "Logan", 2022, "Blanco", "Ideal para transitar en la ciudad", 120000M, true, new List<string>() { "RenaultLogan.png" }, "Renault", "Carro");
                await AddVehicleAsync("POE666", "Megane", 2021, "Café", "Ideal para transitar en la ciudad", 100000M, true, new List<string>() { "RenaultMegane.png" }, "Renault", "Carro");
                await AddVehicleAsync("PJF934", "Sandero", 2020, "Rojo", "Este vehículo es ideal para viajar con tu pareja", 190000M, true, new List<string>() { "RenaultSandero.png" }, "Renault", "Carro");
                await AddVehicleAsync("LKJ838", "Twingo", 2022, "Naranjado", "Ideal para transitar en la ciudad", 150000M, true, new List<string>() { "RenaultTwingo.png" }, "Renault", "Carro");
                await AddVehicleAsync("JIP93F", "Address", 2021, "Blanco", "Ideal para trabajar en la ciudad", 45000M, true, new List<string>() { "SuzukiAddress.png" }, "Suzuki", "Moto");
                await AddVehicleAsync("UHA45F", "DR 150", 2021, "Negro", "Ideal para trabajar en la ciudad", 35000M, true, new List<string>() { "SuzukiDR.png" }, "Suzuki", "Moto");
                await AddVehicleAsync("CVR75F", "Gixxer", 2021, "Gris", "Ideal para realizar el trabajo", 45000M, true, new List<string>() { "SuzukiGixxer.png" }, "Suzuki", "Moto");
                await AddVehicleAsync("UDG75F", "GSX-R", 2022, "Azul", "Ideal para transitar en la ciudad", 60000M, true, new List<string>() { "SuzukiGSX-R.png" }, "Suzuki", "Moto");
                await AddVehicleAsync("UFN754", "Ignis", 2021, "Blanco", "Ideal para trabajar en la ciudad", 450000M, true, new List<string>() { "SuzukiIgnis.png" }, "Suzuki", "Carro");
                await AddVehicleAsync("JIP83F", "New Best", 2021, "Negro", "Ideal para transitar en la ciudad", 45000M, true, new List<string>() { "SuzukiNewBest.png" }, "Suzuki", "Moto");
                await AddVehicleAsync("DEW654", "Swift", 2022, "Amarillo", "El mejor vehículo para transportarse entre departamentos / estados", 190000M, true, new List<string>() { "SuzukiSwift.png" }, "Suzuki", "Carro");
                await AddVehicleAsync("ASO129", "Vitara", 2022, "Rojo", "Este vehículo es ideal para viajar con tu pareja", 450000M, true, new List<string>() { "SuzukiVitara.png" }, "Suzuki", "Carro");
                await AddVehicleAsync("GHJ56D", "V Strom 650", 2022, "Amarillo / Negro", "Ideal para trabajar en la ciudad", 85000M, true, new List<string>() { "SuzukiVStrom.png" }, "Suzuki", "Moto");
                await AddVehicleAsync("GFW95F", "Apache 200", 2022, "Negro", "Ideal para transitar en la ciudad", 105000M, true, new List<string>() { "TVSApache.png" }, "TVS", "Moto");
                await AddVehicleAsync("BJS12F", "NTorq", 2022, "Gris", "Ideal para trabajar en la ciudad", 95000M, true, new List<string>() { "TVSNTorq.png" }, "TVS", "Moto");
                await AddVehicleAsync("JFS45F", "Raider", 2022, "Morado / Negro", "Ideal para trabajar en la ciudad", 85000M, true, new List<string>() { "TVSRaider.png" }, "TVS", "Moto");
                await AddVehicleAsync("ASD95F", "Sport", 2022, "Negro", "Este vehículo es ideal para viajar con tu pareja", 85000M, true, new List<string>() { "TVSSport.png" }, "TVS", "Moto");
                await AddVehicleAsync("JGK53F", "Stryker", 2022, "Azul", "Confortable para la familia", 98000M, true, new List<string>() { "TVSStryker.png" }, "TVS", "Moto");
                await _context.SaveChangesAsync();
            }
        }

        private async Task AddVehicleAsync(
            string plaque, 
            string line, 
            int model, 
            string color, 
            string description, 
            decimal price, 
            bool isRent, 
            List<string> images, 
            string brand, 
            string vehicleType)
        {
            Vehicle vehicle = new()
            {
                Plaque = plaque,
                Line = line,
                Model = model,
                Color = color,
                Description = description,
                Price = price,
                IsRent = isRent,
                Brand = await _context.Brands.FirstOrDefaultAsync(b => b.Name == brand),
                VehicleType = await _context.VehicleTypes.FirstOrDefaultAsync(vt => vt.Name == vehicleType),
                VehiclePhotos = new List<VehiclePhoto>()
            };

            foreach (string image in images)
            {
                Guid imageId = await _blobHelper.UploadBlobAsync($"{Environment.CurrentDirectory}\\wwwroot\\images\\vehicles\\{image}", "products");
                vehicle.VehiclePhotos.Add(new VehiclePhoto { ImageId = imageId });
            }

            _context.Vehicles.Add(vehicle);

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
                _context.Brands.Add(new Brand { Name = "Ford" });
                _context.Brands.Add(new Brand { Name = "Audi" });
                _context.Brands.Add(new Brand { Name = "BMW" });
                _context.Brands.Add(new Brand { Name = "Mercedes Benz" });
                await _context.SaveChangesAsync();
            }
        }
    }
}
