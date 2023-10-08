using LoCoMPro_LV.Models;
using System.Diagnostics;

namespace LoCoMPro_LV.Data
{
    public static class DbInitializer
    {
        public static void Initialize(LoComproContext context)
        {

            if (context.Provinces.Any())
            {
                return;   // DB has been seeded
            }
            if (context.Cantons.Any())
            {
                return;   // DB has been seeded
            }

            var provinces = new Province[]
            {
                new Province{NameProvince = "San José"},
                new Province{NameProvince = "Alajuela"},
                new Province{NameProvince = "Heredia"},
                new Province{NameProvince = "Cartago"},
                new Province{NameProvince = "Puntarenas"},
                new Province{NameProvince = "Guanacaste"},
                new Province{NameProvince = "Limón"}
            };

            context.Provinces.AddRange(provinces);
            context.SaveChanges();

            var cantons = new Canton[]
            {
                new Canton{NameCanton = "San José", Province = provinces[0], NameProvince = "San José"},
                new Canton{NameCanton = "Escazú", Province = provinces[0], NameProvince = "San José"},
                new Canton{NameCanton = "Desamparados", Province = provinces[0], NameProvince = "San José"},
                new Canton{NameCanton = "Puriscal", Province = provinces[0], NameProvince = "San José"},
                new Canton{NameCanton = "Tarrazú", Province = provinces[0], NameProvince = "San José"},
                new Canton{NameCanton = "Aserrí", Province = provinces[0], NameProvince = "San José"},
                new Canton{NameCanton = "Mora", Province = provinces[0], NameProvince = "San José"},
                new Canton{NameCanton = "Goicoechea", Province = provinces[0], NameProvince = "San José"},
                new Canton{NameCanton = "Santa Ana", Province = provinces[0], NameProvince = "San José"},
                new Canton{NameCanton = "Alajuelita", Province = provinces[0], NameProvince = "San José"},
                new Canton{NameCanton = "Vásquez de Coronado", Province = provinces[0], NameProvince = "San José"},
                new Canton{NameCanton = "Acosta", Province = provinces[0], NameProvince = "San José"},
                new Canton{NameCanton = "Tibás", Province = provinces[0], NameProvince = "San José"},
                new Canton{NameCanton = "Moravia", Province = provinces[0], NameProvince = "San José"},
                new Canton{NameCanton = "Montes de Oca", Province = provinces[0], NameProvince = "San José"},
                new Canton{NameCanton = "Turrubares", Province = provinces[0], NameProvince = "San José"},
                new Canton{NameCanton = "Dota", Province = provinces[0], NameProvince = "San José"},
                new Canton{NameCanton = "Curridabat", Province = provinces[0], NameProvince = "San José"},
                new Canton{NameCanton = "Pérez Zeledón", Province = provinces[0], NameProvince = "San José"},
                new Canton{NameCanton = "León Cortés", Province = provinces[0], NameProvince = "San José"},
                new Canton{NameCanton = "San Carlos", Province = provinces[1], NameProvince = "Alajuela"},
                new Canton{NameCanton = "Upala", Province = provinces[1], NameProvince = "Alajuela"},
                new Canton{NameCanton = "Los Chiles", Province = provinces[1], NameProvince = "Alajuela"},
                new Canton{NameCanton = "San Ramón", Province = provinces[1], NameProvince = "Alajuela"},
                new Canton{NameCanton = "Guatuso", Province = provinces[1], NameProvince = "Alajuela"},
                new Canton{NameCanton = "Alajuela", Province = provinces[1], NameProvince = "Alajuela"},
                new Canton{NameCanton = "Rio Cuarto", Province = provinces[1], NameProvince = "Alajuela"},
                new Canton{NameCanton = "Zarcero", Province = provinces[1], NameProvince = "Alajuela"},
                new Canton{NameCanton = "Orotina", Province = provinces[1], NameProvince = "Alajuela"},
                new Canton{NameCanton = "Grecia", Province = provinces[1], NameProvince = "Alajuela"},
                new Canton{NameCanton = "Atenas", Province = provinces[1], NameProvince = "Alajuela"},
                new Canton{NameCanton = "Naranjo", Province = provinces[1], NameProvince = "Alajuela"},
                new Canton{NameCanton = "San Mateo", Province = provinces[1], NameProvince = "Alajuela"},
                new Canton{NameCanton = "Sarchí", Province = provinces[1], NameProvince = "Alajuela"},
                new Canton{NameCanton = "Poás", Province = provinces[1], NameProvince = "Alajuela"},
                new Canton{NameCanton = "Palmares", Province = provinces[1], NameProvince = "Alajuela"},
                new Canton{NameCanton = "Heredia", Province = provinces[2], NameProvince = "Heredia"},
                new Canton{NameCanton = "Barva", Province = provinces[2], NameProvince = "Heredia"},
                new Canton{NameCanton = "Santo Domingo", Province = provinces[2], NameProvince = "Heredia"},
                new Canton{NameCanton = "Santa Bárbara", Province = provinces[2], NameProvince = "Heredia"},
                new Canton{NameCanton = "San Rafael", Province = provinces[2], NameProvince = "Heredia"},
                new Canton{NameCanton = "San Isidro", Province = provinces[2], NameProvince = "Heredia"},
                new Canton{NameCanton = "Belén", Province = provinces[2], NameProvince = "Heredia"},
                new Canton{NameCanton = "Flores", Province = provinces[2], NameProvince = "Heredia"},
                new Canton{NameCanton = "San Pablo", Province = provinces[2], NameProvince = "Heredia"},
                new Canton{NameCanton = "Sarapiquí", Province = provinces[2], NameProvince = "Heredia"},
                new Canton{NameCanton = "Cartago", Province = provinces[3], NameProvince = "Cartago"},
                new Canton{NameCanton = "Paraíso", Province = provinces[3], NameProvince = "Cartago"},
                new Canton{NameCanton = "La Unión", Province = provinces[3], NameProvince = "Cartago"},
                new Canton{NameCanton = "Jiménez", Province = provinces[3], NameProvince = "Cartago"},
                new Canton{NameCanton = "Turrialba", Province = provinces[3], NameProvince = "Cartago"},
                new Canton{NameCanton = "Alvarado", Province = provinces[3], NameProvince = "Cartago"},
                new Canton{NameCanton = "Oreamuno", Province = provinces[3], NameProvince = "Cartago"},
                new Canton{NameCanton = "El Guarco", Province = provinces[3], NameProvince = "Cartago"},
                new Canton{NameCanton = "Puntarenas", Province = provinces[4], NameProvince = "Puntarenas"},
                new Canton{NameCanton = "Esparza", Province = provinces[4], NameProvince = "Puntarenas"},
                new Canton{NameCanton = "Buenos Aires", Province = provinces[4], NameProvince = "Puntarenas"},
                new Canton{NameCanton = "Montes de Oro", Province = provinces[4], NameProvince = "Puntarenas"},
                new Canton{NameCanton = "Osa", Province = provinces[4], NameProvince = "Puntarenas"},
                new Canton{NameCanton = "Quepos", Province = provinces[4], NameProvince = "Puntarenas"},
                new Canton{NameCanton = "Golfito", Province = provinces[4], NameProvince = "Puntarenas"},
                new Canton{NameCanton = "Coto Brus", Province = provinces[4], NameProvince = "Puntarenas"},
                new Canton{NameCanton = "Parrita", Province = provinces[4], NameProvince = "Puntarenas"},
                new Canton{NameCanton = "Corredores", Province = provinces[4], NameProvince = "Puntarenas"},
                new Canton{NameCanton = "Garabito", Province = provinces[4], NameProvince = "Puntarenas"},
                new Canton{NameCanton = "Puerto Jiménez", Province = provinces[4], NameProvince = "Puntarenas"},
                new Canton{NameCanton = "Monte Verde", Province = provinces[4], NameProvince = "Puntarenas"},
                new Canton{NameCanton = "Liberia", Province = provinces[5], NameProvince = "Guanacaste"},
                new Canton{NameCanton = "Nicoya", Province = provinces[5], NameProvince = "Guanacaste"},
                new Canton{NameCanton = "Santa Cruz", Province = provinces[5], NameProvince = "Guanacaste"},
                new Canton{NameCanton = "Bagaces", Province = provinces[5], NameProvince = "Guanacaste"},
                new Canton{NameCanton = "Carrillo", Province = provinces[5], NameProvince = "Guanacaste"},
                new Canton{NameCanton = "Cañas", Province = provinces[5], NameProvince = "Guanacaste"},
                new Canton{NameCanton = "Abangares", Province = provinces[5], NameProvince = "Guanacaste"},
                new Canton{NameCanton = "Tilarán", Province = provinces[5], NameProvince = "Guanacaste"},
                new Canton{NameCanton = "Nandayure", Province = provinces[5], NameProvince = "Guanacaste"},
                new Canton{NameCanton = "La Cruz", Province = provinces[5], NameProvince = "Guanacaste"},
                new Canton{NameCanton = "Hojancha", Province = provinces[5], NameProvince = "Guanacaste"},
                new Canton{NameCanton = "Limón", Province = provinces[6], NameProvince = "Limón"},
                new Canton{NameCanton = "Pococí", Province = provinces[6], NameProvince = "Limón"},
                new Canton{NameCanton = "Siquirres", Province = provinces[6], NameProvince = "Limón"},
                new Canton{NameCanton = "Talamanca", Province = provinces[6], NameProvince = "Limón"},
                new Canton{NameCanton = "Matina", Province = provinces[6], NameProvince = "Limón"},
                new Canton{NameCanton = "Guácimo", Province = provinces[6], NameProvince = "Limón"}
            };

            context.Cantons.AddRange(cantons);
            context.SaveChanges();

            var products = new Product[]
            {
                new Product{NameProduct = "Apple Iphone 11 64gb"},
                new Product{NameProduct = "Apple Iphone 12 64gb"},
                new Product{NameProduct = "Apple Iphone 13 128gb"},
                new Product{NameProduct = "Terreneitor"},
                new Product{NameProduct = "Pantalón bershka gris"}
            };

            context.Products.AddRange(products);
            context.SaveChanges();

            var stores = new Store[]
            {
                new Store{NameStore = "Ishop", Canton = cantons[12],NameProvince = "San José", NameCanton = "Tibás"},
                new Store{NameStore = "Bershka", Canton = cantons[36], NameProvince = "Heredia", NameCanton = "Heredia"},
                new Store{NameStore = "Toys", Canton = cantons[67], NameProvince = "Guanacaste", NameCanton = "Liberia"},
                new Store{NameStore = "Ishop", Canton = cantons[54], NameProvince = "Puntarenas", NameCanton = "Puntarenas"},
                new Store{NameStore = "Bershka", Canton = cantons[27], NameProvince = "Alajuela", NameCanton = "Zarcero"},
                new Store{NameStore = "Toys", Canton = cantons[50], NameProvince = "Cartago", NameCanton = "Turrialba"},
                new Store{NameStore = "Ishop", Canton = cantons[83], NameProvince = "Limón", NameCanton = "Guácimo"},
                new Store{NameStore = "Bershka", Canton = cantons[3], NameProvince = "San José", NameCanton = "Desamparados"},
                new Store{NameStore = "Toys", Canton = cantons[69], NameProvince = "Guanacaste", NameCanton = "Santa Cruz"},
                new Store{NameStore = "Ishop", Canton = cantons[38], NameProvince = "Heredia", NameCanton = "Santo Domingo"}
            };

            context.Stores.AddRange(stores);
            context.SaveChanges();
            
            var application_user = new ApplicationUser[]
            {
                new ApplicationUser{
                    UserName = "brad",
                    FirstName = "Brad",
                    LastName = "Pitt",
                    NameProvince = null,
                    NameCanton = null,
                    NormalizedUserName = "BRAD",
                    Email = "brad@gmail.com",
                    NormalizedEmail = "BRAD@GMAIL.COM",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAIAAYagAAAAEJ5RzIloBjsekpZMd9peZ5256xJJnUEz1HH6sUzx0+ZhzbKjemBcNfDe86+EywvYMQ==",
                    SecurityStamp = "JMSCHDHZEEXXXSXHJR2KHTDX5TJTFLYK",
                    ConcurrencyStamp = "d06538c8-039c-49db-9dde-6c6f03af24d1",
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0},
                new ApplicationUser{ 
                    UserName = "cristopher",
                    FirstName = "Cristopher",
                    LastName = "Hernandez",
                    NameProvince = null,
                    NameCanton = null,
                    NormalizedUserName = "CRISTOPHER",
                    Email = "cristopher@gmail.com",
                    NormalizedEmail = "CRISTOPHER@GMAIL.COM",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAIAAYagAAAAENTfJLP+vTZu1ossokvfaJUf4N9QBbxnHdhVdwi8z2zQheZ6Ii2UYIEi628X6g1cuA==",
                    SecurityStamp = "O7DFZTDZJU7N2B73P5SJCAE6OSGC7BJQ",
                    ConcurrencyStamp = "b0b3cd6d-b488-47f7-8b4d-ac2229c2f71c",
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0},
                new ApplicationUser{
                    UserName = "gabriel",
                    FirstName = "Gabriel",
                    LastName = "Gonzalez",
                    NameProvince = null,
                    NameCanton = null,
                    NormalizedUserName = "GABRIEL",
                    Email = "gabriel@gmail.com",
                    NormalizedEmail = "GABRIEL@GMAIL.COM",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAIAAYagAAAAEHQ/7P4XJNTEenXdJdhsXNCptCv2oQNJl7VKFxlD2nwxa+4tweIH8W+Re5e12IG45w==",
                    SecurityStamp = "CFXPE6LKDNIKQC227NYDKF6LOGWBIZ5P",
                    ConcurrencyStamp = "f9d0ef4f-d297-453e-9cd9-8bfe04ad86e5",
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0},
                new ApplicationUser{
                    UserName = "james",
                    FirstName = "James",
                    LastName = "Araya",
                    NameProvince = null,
                    NameCanton = null,
                    NormalizedUserName = "JAMES",
                    Email = "james@gmail.com",
                    NormalizedEmail = "JAMES@GMAIL.COM",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAIAAYagAAAAEODzrpaQB9lqiPP60kbMkCyJXrKLav+Mu+rgpXJ5MZ9easiDJMP5ZgXUVwZNrOshIQ==",
                    SecurityStamp = "K33HXXIBMPKRTMASCWGYQAKRIMK6L5CD",
                    ConcurrencyStamp = "3c4dd7a8-aa0a-49de-944f-d9ef3d0a11fa",
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0},
                new ApplicationUser{
                    UserName = "keanu",
                    FirstName = "Keanu",
                    LastName = "Reeves",
                    NameProvince = null,
                    NameCanton = null,
                    NormalizedUserName = "KEANU",
                    Email = "keanu@gmail.com",
                    NormalizedEmail = "KEANU@GMAIL.COM",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAIAAYagAAAAEJf0tEnbHCjHl16efwSHZdpBzWf16tYEeSVCvts/deoLVxE86KaL68U3q4sRZIYMyQ==",
                    SecurityStamp = "YIOHF4OTRKR26FEYUY7UGXCZGUNNBHHO",
                    ConcurrencyStamp = "cf541607-c545-4df7-8b0c-a4e7f483d43a",
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0},
                new ApplicationUser{
                    UserName = "natalie",
                    FirstName = "Natalie",
                    LastName = "Portman",
                    NameProvince = null,
                    NameCanton = null,
                    NormalizedUserName = "NATALIE",
                    Email = "natalie@gmail.com",
                    NormalizedEmail = "NATALIE@GMAIL.COM",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAIAAYagAAAAEHDy8HJRmnq9nfP9deB+7ISV6mc1jv+W8p1HdRx9IJOSWXsu8Vd6JrKk5DzEzf+JEQ==",
                    SecurityStamp = "M474D433ZIFNWAYZ6ANT6I4Y53NER7RG",
                    ConcurrencyStamp = "916cd553-5e77-4f04-8018-6f44526dd430",
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0},
                new ApplicationUser{
                    UserName = "sebastian",
                    FirstName = "Sebastian",
                    LastName = "Rodriguez",
                    NameProvince = null,
                    NameCanton = null,
                    NormalizedUserName = "SEBASTIAN",
                    Email = "sebastian@gmail.com",
                    NormalizedEmail = "SEBASTIAN@GMAIL.COM",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAIAAYagAAAAEHPcf4o9Z7feqGV7YaItN1UYJ1f/T2SID6atd8aBVGSJmw3RFNXPbtPs/WNC3QzFVw==",
                    SecurityStamp = "W647FFGZM7EUD775KW4WXZBQPKIFL7S6",
                    ConcurrencyStamp = "fc796c46-b694-4397-af46-125c66c81cd3",
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0},
                new ApplicationUser{
                    UserName = "yordi",
                    FirstName = "Yordi",
                    LastName = "Robles",
                    NameProvince = null,
                    NameCanton = null,
                    NormalizedUserName = "YORDI",
                    Email = "yordi@gmail.com",
                    NormalizedEmail = "YORDI@GMAIL.COM",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAIAAYagAAAAEAeiYSldoIgxbVZnCzpgYilZs+1b8kar9opkK75DnKd6ctZJog304dBUPn9VTjUsvQ==",
                    SecurityStamp = "HX5GAAAYR2BF2",
                    ConcurrencyStamp = "28537e80-4d70-45c3-978c-6910b75e226f",
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0},
                new ApplicationUser{
                    UserName = "emma",
                    FirstName = "Emma",
                    LastName = "Watson",
                    NameProvince = null,
                    NameCanton = null,
                    NormalizedUserName = "EMMA",
                    Email = "emma@gmail.com",
                    NormalizedEmail = "EMMA@GMAIL.COM",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAIAAYagAAAAEEyX2X27Yw1FlQyo88X3zTPpnCrGxjZXOP2eNp+d4X0w42kTb36YXG7HLEWytyWRQA==",
                    SecurityStamp = "74JFSDZ27I2I6D75TJU3TVZHJPOZFSFK",
                    ConcurrencyStamp = "bfa9a3fd-a3ce-4a95-a681-2d9f2b687e92",
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0},
                new ApplicationUser{
                    UserName = "anne",
                    FirstName = "Anne",
                    LastName = "Hathaway",
                    NameProvince = null,
                    NameCanton = null,
                    NormalizedUserName = "ANNE",
                    Email = "anne@gmail.com",
                    NormalizedEmail = "ANNE@GMAIL.COM",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAIAAYagAAAAEJmsnmp+Rm7C6VMHu1s21eBFFButNJUpJ6E6yV5OnERn6c2Hv7KutoQrAaUPyez1lQ==",
                    SecurityStamp = "5NFRN2JU2Z7T7WPCJZ4LY4TA45YHLSXW",
                    ConcurrencyStamp = "cd77e5d0-740a-43f8-9ad4-3d8633ec6d01",
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0},
            };
            context.ApplicationUsers.AddRange(application_user);
            context.SaveChanges();

            var generator_user = new GeneratorUser[]{
                new GeneratorUser{UserName = "brad", ApplicationUser = application_user[0]},
                new GeneratorUser{UserName = "cristopher", ApplicationUser = application_user[1]},
                new GeneratorUser{UserName = "gabriel", ApplicationUser = application_user[2]},
                new GeneratorUser{UserName = "james", ApplicationUser = application_user[3]},
                new GeneratorUser{UserName = "keanu", ApplicationUser = application_user[4]},
                new GeneratorUser{UserName = "natalie", ApplicationUser = application_user[5]},
                new GeneratorUser{UserName = "sebastian", ApplicationUser = application_user[6]},
                new GeneratorUser{UserName = "yordi", ApplicationUser = application_user[7]},
                new GeneratorUser{UserName = "emma", ApplicationUser = application_user[8]},
                new GeneratorUser{UserName = "anne", ApplicationUser = application_user[9]}
            };

            context.GeneratorUsers.AddRange(generator_user);
            context.SaveChanges();

            var records = new Record[]{
                new Record{NameGenerator="anne", GeneratorUser = generator_user[9], RecordDate = DateTime.Parse("2022-1-18"), Price = 250000.66,
                    NameStore = "Ishop", NameProvince = "San José", NameCanton = "Tibás", NameProduct = "Apple Iphone 11 64gb", Store = stores[0], Product = products[0]},

                new Record{NameGenerator="brad", GeneratorUser = generator_user[0], RecordDate = DateTime.Parse("2023-8-27"), Price = 275000,
                    NameStore = "Ishop", NameProvince = "Puntarenas", NameCanton = "Puntarenas", NameProduct = "Apple Iphone 11 64gb", Store = stores[3], Product = products[0]},

                new Record{NameGenerator="cristopher", GeneratorUser = generator_user[1], RecordDate = DateTime.Parse("2021-3-11"), Price = 255000,
                    NameStore = "Ishop", NameProvince = "San José", NameCanton = "Tibás", NameProduct = "Apple Iphone 11 64gb", Store = stores[0], Product = products[0]},

                new Record{NameGenerator="emma", GeneratorUser = generator_user[8], RecordDate = DateTime.Parse("2022-2-12"), Price = 350000,
                    NameStore = "Ishop", NameProvince = "San José", NameCanton = "Tibás", NameProduct = "Apple Iphone 12 64gb", Store = stores[0], Product = products[1]},

                new Record{NameGenerator="gabriel", GeneratorUser = generator_user[2],  RecordDate = DateTime.Parse("2021-2-12"), Price = 375000,
                    NameStore = "Ishop", NameProvince = "San José", NameCanton = "Tibás", NameProduct = "Apple Iphone 12 64gb", Store = stores[0], Product = products[1]},

                new Record{NameGenerator="james",GeneratorUser = generator_user[3],  RecordDate = DateTime.Parse("2021-2-12"), Price = 455000,
                    NameStore = "Ishop", NameProvince = "Limón", NameCanton = "Guácimo", NameProduct = "Apple Iphone 13 128gb", Store = stores[6], Product = products[2]},

                new Record{NameGenerator="keanu", GeneratorUser = generator_user[4], RecordDate = DateTime.Parse("2023-4-25"), Price = 50000,
                    NameStore = "Toys", NameProvince = "Cartago", NameCanton = "Turrialba", NameProduct = "Terreneitor", Store = stores[5], Product = products[3], Description="TERRENEITOR, el coche más poderoso que ha existido."},

                new Record{NameGenerator="natalie",GeneratorUser = generator_user[5],  RecordDate = DateTime.Parse("2023-5-10"), Price = 70000,
                    NameStore = "Toys", NameProvince = "Cartago", NameCanton = "Turrialba", NameProduct = "Terreneitor", Store = stores[5], Product = products[3]},

                new Record{NameGenerator="sebastian",GeneratorUser = generator_user[6], RecordDate = DateTime.Parse("2023-9-7"), Price = 60000,
                    NameStore = "Toys", NameProvince = "Guanacaste", NameCanton = "Santa Cruz", NameProduct = "Terreneitor", Store = stores[8], Product = products[3]},

                new Record{NameGenerator="yordi",GeneratorUser = generator_user[7], RecordDate = DateTime.Parse("2022-2-15"), Price = 20000,
                    NameStore = "Bershka", NameProvince = "Heredia", NameCanton = "Heredia", NameProduct = "Pantalón bershka gris", Store = stores[1], Product = products[4]},

                new Record{NameGenerator="natalie",GeneratorUser = generator_user[5], RecordDate = DateTime.Parse("2022-2-21"), Price = 22000,
                    NameStore = "Bershka", NameProvince = "Heredia", NameCanton = "Heredia", NameProduct = "Pantalón bershka gris", Store = stores[1], Product = products[4]},

                new Record{NameGenerator="emma",GeneratorUser = generator_user[8], RecordDate = DateTime.Parse("2022-2-16"), Price = 27000,
                    NameStore = "Bershka", NameProvince = "Alajuela", NameCanton = "Zarcero", NameProduct = "Pantalón bershka gris", Store = stores[4], Product = products[4]},

                new Record{NameGenerator="james",GeneratorUser = generator_user[3], RecordDate = DateTime.Parse("2022-3-4"), Price = 23000,
                    NameStore = "Bershka", NameProvince = "San José", NameCanton = "Desamparados", NameProduct = "Pantalón bershka gris", Store = stores[7], Product = products[4]},
            };
            context.Records.AddRange(records);
            context.SaveChanges();

            var categories = new Category[]
            {
                new Category{NameCategory = "Carros", NameTopCategory = "Juguetes"},
                new Category{NameCategory = "Ropa", NameTopCategory = "Moda"},
                new Category{NameCategory = "Pantalon", NameTopCategory = "Ropa"},
                new Category{NameCategory = "Celulares", NameTopCategory = "Tecnologia"},
                new Category{NameCategory = "Juguetes"},
                new Category{NameCategory = "Moda"},
                new Category{NameCategory = "Tecnologia"}
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();

            var associated = new Associated[]
            {
                new Associated{NameProduct = "Apple Iphone 11 64gb" , NameCategory = "Celulares", Product = products[0], Category = categories[3]},
                new Associated{NameProduct = "Apple Iphone 12 64gb" , NameCategory = "Celulares", Product = products[1], Category = categories[3]},
                new Associated{NameProduct = "Apple Iphone 13 128gb" , NameCategory = "Celulares", Product = products[2], Category = categories[3]},
                new Associated{NameProduct = "Terreneitor" , NameCategory = "Carros", Product = products[3], Category = categories[0]},
                new Associated{NameProduct = "Pantalón bershka gris" , NameCategory = "Pantalon", Product = products[4], Category = categories[1]}
            };
            context.Associated.AddRange(associated);
            context.SaveChanges();
        }
    }
}