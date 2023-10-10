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
                new Product{NameProduct = "Pantalón bershka gris"},
                new Product{NameProduct = "Hamburguesa con queso"},
                new Product{NameProduct = "Colgate Pasta Total 12 Clean Mint 75ml"},
                new Product{NameProduct = "Camisa original Real Madrid #5 Bellingham"},
                new Product{NameProduct = "Four loko Mango 500ml"},
                new Product{NameProduct = "Samsung Galaxy S23 ultra prime"},
                new Product{NameProduct = "Colgate Pasta Total 12 Clean Mint 200ml"},
                new Product{NameProduct = "ASEPXIA JABÓN BARRA AZUFRE 100 GR"}
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
                new Store{NameStore = "Ishop", Canton = cantons[38], NameProvince = "Heredia", NameCanton = "Santo Domingo"},
                new Store{NameStore = "McDonald's", Canton = cantons[82], NameProvince = "Limón", NameCanton = "Matina"},
                new Store{NameStore = "McDonald's", Canton = cantons[56], NameProvince = "Puntarenas", NameCanton = "Buenos Aires"},
                new Store{NameStore = "Fischel", Canton = cantons[1], NameProvince = "San José", NameCanton = "Escazú"},
                new Store{NameStore = "Fischel", Canton = cantons[13], NameProvince = "San José", NameCanton = "Moravia"},
                new Store{NameStore = "Match On", Canton = cantons[0], NameProvince = "San José", NameCanton = "San José" },
                new Store{NameStore = "Supercamisas", Canton = cantons[1], NameProvince = "San José", NameCanton = "Escazú" },
                new Store{NameStore = "Mas X Menos", Canton = cantons[2], NameProvince = "San José", NameCanton = "Desamparados" },
                new Store{NameStore = "Pali", Canton = cantons[20], NameProvince = "Alajuela", NameCanton = "San Carlos" },
                new Store{NameStore = "AM/PM", Canton = cantons[21], NameProvince = "Alajuela", NameCanton = "Upala" },
                new Store{NameStore = "Farmacia La Bomba", Canton = cantons[22], NameProvince = "Alajuela", NameCanton = "Los Chiles" },
                new Store{NameStore = "Extremetech", Canton = cantons[36], NameProvince = "Heredia", NameCanton = "Heredia" },
                new Store{NameStore = "Monge", Canton = cantons[37], NameProvince = "Heredia", NameCanton = "Barva" },
                new Store{NameStore = "Match On", Canton = cantons[38], NameProvince = "Heredia", NameCanton = "Santo Domingo" },
                new Store{NameStore = "Supercamisas", Canton = cantons[46], NameProvince = "Cartago", NameCanton = "Cartago" },
                new Store{NameStore = "Mas X Menos", Canton = cantons[47], NameProvince = "Cartago", NameCanton = "Paraíso" },
                new Store{NameStore = "Pali", Canton = cantons[48], NameProvince = "Cartago", NameCanton = "La Unión" },
                new Store{NameStore = "AM/PM", Canton = cantons[54], NameProvince = "Puntarenas", NameCanton = "Puntarenas" },
                new Store{NameStore = "Farmacia La Bomba", Canton = cantons[55], NameProvince = "Puntarenas", NameCanton = "Esparza" },
                new Store{NameStore = "Extremetech", Canton = cantons[56], NameProvince = "Puntarenas", NameCanton = "Buenos Aires" },
                new Store{NameStore = "Monge", Canton = cantons[67], NameProvince = "Guanacaste", NameCanton = "Liberia" },
                new Store{NameStore = "Match On", Canton = cantons[68], NameProvince = "Guanacaste", NameCanton = "Nicoya" },
                new Store{NameStore = "Supercamisas", Canton = cantons[69], NameProvince = "Guanacaste", NameCanton = "Santa Cruz" },
                new Store{NameStore = "Mas X Menos", Canton = cantons[78], NameProvince = "Limón", NameCanton = "Limón" },
                new Store{NameStore = "Pali", Canton = cantons[79], NameProvince = "Limón", NameCanton = "Pococí" },
                new Store{NameStore = "AM/PM", Canton = cantons[80], NameProvince = "Limón", NameCanton = "Siquirres" }
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
                new Record{NameGenerator="anne", GeneratorUser = generator_user[9], RecordDate = DateTime.Parse("2022-1-18"), Price = 280000.5643,
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

                new Record{NameGenerator="sebastian",GeneratorUser = generator_user[6], RecordDate = DateTime.Parse("2023-9-7"), Price = 60000.453446,
                    NameStore = "Toys", NameProvince = "Guanacaste", NameCanton = "Santa Cruz", NameProduct = "Terreneitor", Store = stores[8], Product = products[3]},

                new Record{NameGenerator="yordi",GeneratorUser = generator_user[7], RecordDate = DateTime.Parse("2022-2-15"), Price = 20000,
                    NameStore = "Bershka", NameProvince = "Heredia", NameCanton = "Heredia", NameProduct = "Pantalón bershka gris", Store = stores[1], Product = products[4]},

                new Record{NameGenerator="natalie",GeneratorUser = generator_user[5], RecordDate = DateTime.Parse("2022-2-21"), Price = 22000,
                    NameStore = "Bershka", NameProvince = "Heredia", NameCanton = "Heredia", NameProduct = "Pantalón bershka gris", Store = stores[1], Product = products[4]},

                new Record{NameGenerator="emma",GeneratorUser = generator_user[8], RecordDate = DateTime.Parse("2022-2-16"), Price = 27000,
                    NameStore = "Bershka", NameProvince = "Alajuela", NameCanton = "Zarcero", NameProduct = "Pantalón bershka gris", Store = stores[4], Product = products[4]},

                new Record{NameGenerator="james",GeneratorUser = generator_user[3], RecordDate = DateTime.Parse("2022-3-4"), Price = 23000,
                    NameStore = "Bershka", NameProvince = "San José", NameCanton = "Desamparados", NameProduct = "Pantalón bershka gris", Store = stores[7], Product = products[4]},

                new Record{NameGenerator="gabriel",GeneratorUser = generator_user[2], RecordDate = DateTime.Parse("2023-5-22"), Price = 2500,
                    NameStore = "McDonald's", NameProvince = "Limón", NameCanton = "Matina", NameProduct = "Hamburguesa con queso", Store = stores[10], Product = products[5], Description="La recordaba más barata pero igual de rica"},

                new Record{NameGenerator="yordi",GeneratorUser = generator_user[7], RecordDate = DateTime.Parse("2023-1-18"), Price = 1500,
                    NameStore = "McDonald's", NameProvince = "Limón", NameCanton = "Matina", NameProduct = "Hamburguesa con queso", Store = stores[10], Product = products[5], Description="Muy deliciosa y barata."},

                new Record{NameGenerator="sebastian",GeneratorUser = generator_user[6], RecordDate = DateTime.Parse("2022-11-1"), Price = 1000.201,
                    NameStore = "McDonald's", NameProvince = "Puntarenas", NameCanton = "Buenos Aires", NameProduct = "Hamburguesa con queso", Store = stores[11], Product = products[5], Description="Demasiado barato aunque un poco dura."},

                new Record{NameGenerator="gabriel",GeneratorUser = generator_user[2], RecordDate = DateTime.Parse("2023-8-10"), Price = 2039.95,
                    NameStore = "Fischel", NameProvince = "San José", NameCanton = "Escazú", NameProduct = "Colgate Pasta Total 12 Clean Mint 75ml", Store = stores[12], Product = products[6], Description="Más cara que en otros lugares."},

                new Record{NameGenerator="keanu",GeneratorUser = generator_user[4], RecordDate = DateTime.Parse("2023-8-6"), Price = 2039.95,
                    NameStore = "Fischel", NameProvince = "San José", NameCanton = "Escazú", NameProduct = "Colgate Pasta Total 12 Clean Mint 75ml", Store = stores[12], Product = products[6], Description="Es bastante buena, pero está muy cara en diferencia a otros establecimientos."},

                new Record{NameGenerator="cristopher",GeneratorUser = generator_user[1], RecordDate = DateTime.Parse("2023-8-6"), Price = 1890.50,
                    NameStore = "Fischel", NameProvince = "San José", NameCanton = "Moravia", NameProduct = "Colgate Pasta Total 12 Clean Mint 75ml", Store = stores[13], Product = products[6], Description="La recomiendo, la he utilizado durante 1 año y siempre ha sido mi favorita."},

                new Record{NameGenerator="brad", GeneratorUser = generator_user[0], RecordDate = DateTime.Parse("2023-3-15"), Price = 17000,
                    NameStore = "Match On", NameProvince = "Guanacaste", NameCanton = "Nicoya", NameProduct = "Camisa original Real Madrid #5 Bellingham", Store = stores[30], Product = products[7]},

                new Record{NameGenerator="cristopher", GeneratorUser = generator_user[1], RecordDate = DateTime.Parse("2022-7-20"), Price = 17500,
                    NameStore = "Supercamisas", NameProvince = "Guanacaste", NameCanton = "Santa Cruz", NameProduct = "Camisa original Real Madrid #5 Bellingham", Store = stores[31], Product = products[7]},

                new Record{NameGenerator="sebastian", GeneratorUser = generator_user[6], RecordDate = DateTime.Parse("2022-3-10"), Price = 22000,
                    NameStore = "Match On", NameProvince = "Guanacaste", NameCanton = "Nicoya", NameProduct = "Camisa original Real Madrid #5 Bellingham", Store = stores[30], Product = products[7]},

                new Record{NameGenerator="emma", GeneratorUser = generator_user[8], RecordDate = DateTime.Parse("2022-4-12"), Price = 19000,
                    NameStore = "Supercamisas", NameProvince = "San José", NameCanton = "Escazú", NameProduct = "Camisa original Real Madrid #5 Bellingham", Store = stores[24], Product = products[7]},

                new Record{NameGenerator="yordi", GeneratorUser = generator_user[7], RecordDate = DateTime.Parse("2022-8-25"), Price = 16000,
                    NameStore = "Supercamisas", NameProvince = "Guanacaste", NameCanton = "Santa Cruz", NameProduct = "Camisa original Real Madrid #5 Bellingham", Store = stores[31], Product = products[7]},

                new Record{NameGenerator="sebastian", GeneratorUser = generator_user[6], RecordDate = DateTime.Parse("2022-3-11"), Price = 20000,
                    NameStore = "Match On", NameProvince = "Guanacaste", NameCanton = "Nicoya", NameProduct = "Camisa original Real Madrid #5 Bellingham", Store = stores[30], Product = products[7]},

                new Record{NameGenerator="cristopher", GeneratorUser = generator_user[1], RecordDate = DateTime.Parse("2022-7-28"), Price = 19000,
                    NameStore = "Supercamisas", NameProvince = "Guanacaste", NameCanton = "Santa Cruz", NameProduct = "Camisa original Real Madrid #5 Bellingham", Store = stores[31], Product = products[7]},

                new Record{NameGenerator="brad", GeneratorUser = generator_user[0], RecordDate = DateTime.Parse("2023-3-25"), Price = 20000,
                    NameStore = "Match On", NameProvince = "Guanacaste", NameCanton = "Nicoya", NameProduct = "Camisa original Real Madrid #5 Bellingham", Store = stores[30], Product = products[7]},

                new Record{NameGenerator="james", GeneratorUser = generator_user[3], RecordDate = DateTime.Parse("2022-12-19"), Price = 16000,
                    NameStore = "Match On", NameProvince = "San José", NameCanton = "San José", NameProduct = "Camisa original Real Madrid #5 Bellingham", Store = stores[14], Product = products[7]},

                new Record{NameGenerator="keanu", GeneratorUser = generator_user[4], RecordDate = DateTime.Parse("2022-9-29"), Price = 19000,
                    NameStore = "Match On", NameProvince = "Heredia", NameCanton = "Santo Domingo", NameProduct = "Camisa original Real Madrid #5 Bellingham", Store = stores[4], Product = products[7]},

                new Record{NameGenerator="sebastian", GeneratorUser = generator_user[6], RecordDate = DateTime.Parse("2023-10-9"), Price = 450000,
                    NameStore = "Monge", NameProvince = "Guanacaste", NameCanton = "Liberia", NameProduct = "Samsung Galaxy S23 ultra prime", Store = stores[29], Product = products[9], Description="El mejor celular calidad precio, después de xiaomi"},

                new Record{NameGenerator="keanu", GeneratorUser = generator_user[4], RecordDate = DateTime.Parse("2023-1-18"), Price = 8000,
                    NameStore = "Farmacia La Bomba", NameProvince = "Puntarenas", NameCanton = "Esparza", NameProduct = "ASEPXIA JABÓN BARRA AZUFRE 100 GR", Store = stores[27], Product = products[11], Description="Increíble."},

                new Record{NameGenerator="gabriel", GeneratorUser = generator_user[2], RecordDate = DateTime.Parse("2022-2-7"), Price = 2900,
                    NameStore = "Mas X Menos", NameProvince = "San José", NameCanton = "Desamparados", NameProduct = "Four loko Mango 500ml", Store = stores[16], Product = products[8], Description="La mejor four, y esta 2x1."},

                new Record{NameGenerator="cristopher", GeneratorUser = generator_user[1], RecordDate = DateTime.Parse("2022-2-7"), Price = 2600,
                    NameStore = "Mas X Menos", NameProvince = "San José", NameCanton = "Desamparados", NameProduct = "Four loko Mango 500ml", Store = stores[16], Product = products[8],  Description="Me gusta más la de sandía, pero esta no esta mal."},

                new Record{NameGenerator="james", GeneratorUser = generator_user[3], RecordDate = DateTime.Parse("2021-11-12"), Price = 380000,
                    NameStore = "Ishop", NameProvince = "San José", NameCanton = "Tibás", NameProduct = "Apple Iphone 12 64gb", Store = stores[0], Product = products[1],  Description="Me parece un muy buen celular."},

                new Record{NameGenerator="keanu", GeneratorUser = generator_user[4], RecordDate = DateTime.Parse("2021-9-27"), Price = 1500,
                    NameStore = "Pali", NameProvince = "Limón", NameCanton = "Pococí", NameProduct = "Colgate Pasta Total 12 Clean Mint 75ml", Store = stores[33], Product = products[6], Description=" Colgate grande."},

                new Record{NameGenerator="james",GeneratorUser = generator_user[3], RecordDate = DateTime.Parse("2023-10-10"), Price = 21000,
                    NameStore = "Bershka", NameProvince = "San José", NameCanton = "Heredia", NameProduct = "Pantalón bershka gris", Store = stores[1], Product = products[4], Description=" Descuento de temporada."},

                new Record{NameGenerator="gabriel",GeneratorUser = generator_user[2], RecordDate = DateTime.Parse("2015-8-9"), Price = 1000,
                    NameStore = "Mas X Menos", NameProvince = "Cartago", NameCanton = "Paraíso", NameProduct = "Four loko Mango 500ml", Store = stores[24], Product = products[8], Description=" Promoción por los próximos 3 días."},

                new Record{NameGenerator="sebastian", GeneratorUser = generator_user[6], RecordDate = DateTime.Parse("2018-7-26"), Price = 817425,
                    NameStore = "Monge", NameProvince = "Heredia", NameCanton = "Barva", NameProduct = "Samsung Galaxy S23 ultra prime", Store = stores[21], Product = products[9]},

                new Record{NameGenerator="yordi", GeneratorUser = generator_user[7], RecordDate = DateTime.Parse("2023-1-1"), Price = 1235000,
                    NameStore = "Extremetech", NameProvince = "Heredia", NameCanton = "Heredia", NameProduct = "Apple Iphone 12 64gb", Store = stores[20], Product = products[1]}
            };
            
            context.Records.AddRange(records);
            context.SaveChanges();

            var categories = new Category[]
            {
                new Category{NameCategory = "Carros", NameTopCategory = "Juguetes"},
                new Category{NameCategory = "Ropa", NameTopCategory = "Moda"},
                new Category{NameCategory = "Pantalon", NameTopCategory = "Ropa"},
                new Category{NameCategory = "Celulares", NameTopCategory = "Tecnologia"},
                new Category{NameCategory = "Comida rápida", NameTopCategory = "Consumibles"},
                new Category{NameCategory = "Salud Bucal", NameTopCategory = "Farmacia"},
                new Category{NameCategory = "Juguetes"},
                new Category{NameCategory = "Moda"},
                new Category{NameCategory = "Tecnologia"},
                new Category{NameCategory = "Consumibles"},
                new Category{NameCategory = "Farmacia"},
                new Category{NameCategory = "Ropa deportiva", NameTopCategory = "Ropa"}
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();

            var associated = new Associated[]
            {
                new Associated{NameProduct = "Apple Iphone 11 64gb" , NameCategory = "Celulares", Product = products[0], Category = categories[3]},
                new Associated{NameProduct = "Apple Iphone 12 64gb" , NameCategory = "Celulares", Product = products[1], Category = categories[3]},
                new Associated{NameProduct = "Apple Iphone 13 128gb" , NameCategory = "Celulares", Product = products[2], Category = categories[3]},
                new Associated{NameProduct = "Terreneitor" , NameCategory = "Carros", Product = products[3], Category = categories[0]},
                new Associated{NameProduct = "Pantalón bershka gris" , NameCategory = "Pantalon", Product = products[4], Category = categories[1]},
                new Associated{NameProduct = "Hamburguesa con queso" , NameCategory = "Comida rápida", Product = products[5], Category = categories[4]},
                new Associated{NameProduct = "Colgate Pasta Total 12 Clean Mint 75ml" , NameCategory = "Salud Bucal", Product = products[6], Category = categories[5]},
                new Associated{NameProduct = "Samsung Galaxy S23 ultra prime", NameCategory = "Celulares", Product = products[9], Category = categories[3]},
                new Associated{NameProduct = "ASEPXIA JABÓN BARRA AZUFRE 100 GR", NameCategory = "Farmacia", Product = products[11], Category = categories[10]},
                new Associated{NameProduct = "Four loko Mango 500ml", NameCategory = "Consumibles", Product = products[8], Category = categories[9]},
                new Associated{NameProduct = "Camisa original Real Madrid #5 Bellingham" , NameCategory = "Ropa deportiva", Product = products[7], Category = categories[11]}
            };
            context.Associated.AddRange(associated);
            context.SaveChanges();
        }
    }
}