using ApplicationCore.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class MagShopContextSeed
    {
        #region Define consts
        //Users
        private const string USER_ID = "demoseller@microsoft.com";
        private const string BASKET_ID = "12345";
        private const string CREDIT_CARD_ID = "123";
        //Stores
        private const string SELLER_ID = "1235";
        private const string BASKET_SELLER_ID = "12346";
        private const string CREDIT_CARD_SELLER_ID = "124";
        private const string STORE_ID = "12346";
        //Products
        private const string PRODUCT_1_ID = "product1";
        private const string PRODUCT_2_ID = "product2";
        private const string PRODUCT_3_ID = "product3";
        private const string PRODUCT_4_ID = "product4";
        private const string PRODUCT_5_ID = "product5";
        private const string PRODUCT_6_ID = "product6";
        private const string PRODUCT_7_ID = "product7";
        private const string PRODUCT_8_ID = "product8";

        //Category
        public const string PARENT_ID = "mainCategory";
        private const string CATEGORY_1_ID = "category1";
        private const string CATEGORY_2_ID = "category2";
        private const string CATEGORY_3_ID = "category3";
        private const string CATEGORY_4_ID = "category4";
        private const string CATEGORY_5_ID = "category5";
        private const string CATEGORY_6_ID = "category6";
        #endregion

        public static async Task SeedAsync(MagShopContext context,
            ILoggerFactory loggerFactory, UserManager<UserAuthAccess> userManager,
            RoleManager<IdentityRole> roleManager, int? retry = 0)
        {
            int retryForAvailability = retry.Value;
            try
            {
                // TODO: Only run this if using a real database
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                //context.Database.Migrate();
                if (!await context.Users.AnyAsync())
                {
                    if (!roleManager.Roles.Any() && !userManager.Users.Any())
                    {
                        await roleManager.CreateAsync(new IdentityRole(Constants.ConstantsAPI.ADMINISTRATORS));
                        await roleManager.CreateAsync(new IdentityRole(Constants.ConstantsAPI.SELLERS));
                        await roleManager.CreateAsync(new IdentityRole(Constants.ConstantsAPI.USERS));

                        var defaultUser = new UserAuthAccess("demoseller@microsoft.com");
                        defaultUser.Id = USER_ID;
                        await userManager.CreateAsync(defaultUser, Constants.ConstantsAPI.DEFAULT_PASSWORD);
                        var user = await userManager.FindByNameAsync(defaultUser.UserName);
                        await userManager.AddToRoleAsync(user, Constants.ConstantsAPI.USERS);

                        string adminUserName = "admin@microsoft.com";
                        var adminUser = new UserAuthAccess(adminUserName);
                        adminUser.Id = SELLER_ID;
                        await userManager.CreateAsync(adminUser, Constants.ConstantsAPI.DEFAULT_PASSWORD);
                        adminUser = await userManager.FindByNameAsync(adminUserName);
                        await userManager.AddToRoleAsync(adminUser, Constants.ConstantsAPI.ADMINISTRATORS);


                        await context.Users.AddRangeAsync(
                            GetPreconfiguredUsers(defaultUser.Id, adminUser.Id));
                    }

                    await context.SaveChangesAsync();
                }
                if (!await context.Categories.AnyAsync())
                {
                    await context.Categories.AddRangeAsync(
                        GetPreconfiguredCategories());
                    await context.SaveChangesAsync();
                }
                if (!await context.Stores.AnyAsync())
                {
                    await context.Stores.AddRangeAsync(
                        GetPreconfiguredStores());
                    await context.SaveChangesAsync();
                }

                if (!await context.Products.AnyAsync())
                {
                    await context.Products.AddRangeAsync(
                        GetPrecongifuredProducts());
                    await context.SaveChangesAsync();
                }

                if (!await context.Orders.AnyAsync())
                {
                    AddPrecongifuredOrdersToFirstUser(context);
                    await context.SaveChangesAsync();
                }

                if (await context.Users.Include(x => x.Basket)
                    .ThenInclude(x => x.Items).FirstAsync() != null)
                {
                    AddPreconfiguredBasketItemsToFirstUser(context);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<MagShopContextSeed>();
                    log.LogError(ex.Message);
                    await SeedAsync(context, loggerFactory, userManager, roleManager, retryForAvailability);
                }
                throw;
            }
        }

        private static void AddPreconfiguredBasketItemsToFirstUser(MagShopContext context)
        {
            var user = context.Users.Find(USER_ID);
            var products = context.Products.Where(x => x.StoreId == STORE_ID);
            List<BasketItem> basketItems = new List<BasketItem>();
            foreach (var product in products)
            {
                basketItems.Add(new BasketItem(2, product));
            }
            user.AddItemsToBasket(basketItems);
        }
        #region Categories
        private static IEnumerable<Category> GetPreconfiguredCategories()
        {
            return new List<Category>()
            {
                new Category(PARENT_ID, null, "MAIN_CATEGORY", 0),
                new Category(CATEGORY_1_ID, PARENT_ID, "Технические предметы", 0.2),
                new Category(CATEGORY_2_ID, PARENT_ID, "Лингвестические предметы", 0.2),
                new Category(CATEGORY_3_ID, PARENT_ID, "Биологическое направение", 0.1),
                new Category(CATEGORY_4_ID, PARENT_ID, "Естественные науки", 0.2 ),
                new Category(CATEGORY_5_ID, PARENT_ID, "Программирование", 0.25),
                new Category(CATEGORY_6_ID, PARENT_ID, "Очумелые ручки", 0.311233223)

            };
        }
        #endregion
        #region Products
        private static IEnumerable<Product> GetPrecongifuredProducts()
        {
            var assembly = Assembly.GetExecutingAssembly();
            Stream resource = assembly.GetManifestResourceStream("Infrastructure.Data.Images." + "1.jpg");
            Stream resource1 = assembly.GetManifestResourceStream("Infrastructure.Data.Images." + "2.jpg");
            Stream resource2 = assembly.GetManifestResourceStream("Infrastructure.Data.Images." + "3.jpg");
            Stream resource3 = assembly.GetManifestResourceStream("Infrastructure.Data.Images." + "4.jpg");
            Stream resource4 = assembly.GetManifestResourceStream("Infrastructure.Data.Images." + "5.jpg");
            Stream resource5 = assembly.GetManifestResourceStream("Infrastructure.Data.Images." + "6.jpg");
            Stream resource6 = assembly.GetManifestResourceStream("Infrastructure.Data.Images." + "7.jpg");
            Stream resource7 = assembly.GetManifestResourceStream("Infrastructure.Data.Images." + "8.jpg");
            
            
            MemoryStream ms = new MemoryStream();
            if (resource != null)
            {
                resource.CopyTo(ms);
            }
            var image1 = ms.ToArray();
            if (image1.Length > 0)
            {

            }
            if (resource != null)
            {
                resource.CopyTo(ms);
            }
            var image2 = ms.ToArray();
            if (image2.Length > 0)
            {

            }

            var image3 = ms.ToArray();
            if (image3.Length > 0)
            {

            }

            var image4 = ms.ToArray();
            if (image4.Length > 0)
            {

            }

            var image5 = ms.ToArray();
            if (image5.Length > 0)
            {

            }
            var image6 = ms.ToArray();
            if (image6.Length > 0)
            {

            }

            var image7 = ms.ToArray();
            if (image7.Length > 0)
            {

            }
            var image8 = ms.ToArray();
            if (image8.Length > 0)
            {

            }
            return new List<Product>()
            {
                new Product(PRODUCT_1_ID, "Биология", 180, CATEGORY_3_ID, "Обучит науке о живых организмах",
                    new List<Property>()
                    {
                        new Property("Классы",
                        new List<PropertyItem>(){
                           new PropertyItem("1"){Image = image1 },
                        new PropertyItem("2"){Image = image1 },
                        new PropertyItem("3"){Image = image1 },
                        new PropertyItem("4"){Image = image1 },
                        new PropertyItem("5"){Image = image1 },
                        new PropertyItem("6"){Image = image1 },
                        new PropertyItem("7"){Image = image1 },
                        new PropertyItem("8"){Image = image1 },
                        new PropertyItem("9"){Image = image1 },
                        })
                    },
                    STORE_ID)
                 {
                    PreviewImage = image1,
                    Url = "ewqeqe"
                },

                new Product(PRODUCT_2_ID, "Корушка", 10, CATEGORY_6_ID, "Сдлайте все своими руками",
                new List<Property>()
                {
                    new Property("Типы",
                    new List<PropertyItem>(){
                        new PropertyItem("Для голубей"){Image = image2 },
                        new PropertyItem("Для воробьев"){Image = image2 },
                        new PropertyItem("Для белок"){Image = image2 },
                        new PropertyItem("Для кошек"){Image = image2 },
                    })
                }, STORE_ID)
                {
                    PreviewImage = image2,
                    Url = "ewqeqe"
                },
                new Product(PRODUCT_3_ID, "1с", 18000, CATEGORY_5_ID, "Обучитесь программированию на 1с",
                new List<Property>()
                {
                    new Property("Уровень сложности",
                    new List<PropertyItem>(){
                        new PropertyItem("Минимальный"){Image = image3},
                        new PropertyItem("Нормальный"){Image = image3},
                        new PropertyItem("Продвинутый"){Image = image3},
                        new PropertyItem("Экспертный"){Image = image3},
                    })
                }, STORE_ID)
                {
                    PreviewImage = image3,
                    Url = "ewqeqe"
                },
                new Product(PRODUCT_4_ID, "Физика", 1800, CATEGORY_4_ID, "Обучение основам физики",
                new List<Property>()
                {
                    new Property("Классы",
                    new List<PropertyItem>(){
                       new PropertyItem("1"){Image = image4},
                        new PropertyItem("2"){Image = image4},
                        new PropertyItem("3"){Image = image4},
                        new PropertyItem("4"){Image = image4},
                        new PropertyItem("5"){Image = image4},
                        new PropertyItem("6"){Image = image4},
                        new PropertyItem("7"){Image = image4},
                        new PropertyItem("8"){Image = image4},
                        new PropertyItem("9"){Image = image4},
                    })
                }, STORE_ID)
                {
                    PreviewImage = image4,
                    Url = "ewqeqe"
                },
                new Product(PRODUCT_5_ID, "Французкий язык", 1800, CATEGORY_2_ID, "Научитесь говорить как истинный француз",
                new List<Property>()
                {
                     new Property("Классы",
                    new List<PropertyItem>(){
                       new PropertyItem("1"){Image = image5},
                        new PropertyItem("2"){Image = image5},
                        new PropertyItem("3"){Image = image5},
                        new PropertyItem("4"){Image = image5},
                        new PropertyItem("5"){Image = image5},
                        new PropertyItem("6"){Image = image5},
                        new PropertyItem("7"){Image = image5},
                        new PropertyItem("8"){Image = image5},
                        new PropertyItem("9"){Image = image5},
                    })
                }, STORE_ID+"1")
                {
                    PreviewImage = image5,
                    Url = "ewqeqe"
                },
                new Product(PRODUCT_6_ID, "Математика", 1800, CATEGORY_1_ID, "Научитесть складывать цифры и не только",
                new List<Property>()
                {
                    new Property("Классы",
                    new List<PropertyItem>(){
                        new PropertyItem("1"){Image = image6},
                        new PropertyItem("2"){Image = image6},
                        new PropertyItem("3"){Image = image6},
                        new PropertyItem("4"){Image = image6},
                        new PropertyItem("5"){Image = image6},
                        new PropertyItem("6"){Image = image6},
                        new PropertyItem("7"){Image = image6},
                        new PropertyItem("8"){Image = image6},
                        new PropertyItem("9"){Image = image6},
                    })
                }, STORE_ID+"1")
                {
                    PreviewImage = image6,
                    Url = "ewqeqe"
                },
                new Product(PRODUCT_7_ID, "Русский язык", 1800, CATEGORY_2_ID, "Обучат грамматике",
                new List<Property>()
                {
                    new Property("Классы",
                    new List<PropertyItem>(){
                        new PropertyItem("1"),
                        new PropertyItem("2"),
                        new PropertyItem("3"),
                        new PropertyItem("4"),
                        new PropertyItem("5"),
                        new PropertyItem("6"),
                        new PropertyItem("7"),
                        new PropertyItem("8"),
                        new PropertyItem("9"),
                    })
                }, STORE_ID+"2")
                {
                    PreviewImage = image7,
                    Url = "ewqeqe"
                },
                new Product(PRODUCT_8_ID, "География", 7000, CATEGORY_4_ID, "Научат ориентироваться в картах",
                new List<Property>()
                {
                    new Property("Классы",
                    new List<PropertyItem>(){
                        new PropertyItem("1"),
                        new PropertyItem("2"),
                        new PropertyItem("3"),
                        new PropertyItem("4"),
                        new PropertyItem("5"),
                        new PropertyItem("6"),
                        new PropertyItem("7"),
                        new PropertyItem("8"),
                        new PropertyItem("9"),
                    })
                }, STORE_ID+"2")
                {
                    PreviewImage = image8,
                    Url = "ewqeqe"
                },
            };
        }
        #endregion
        #region Stores
        private static IEnumerable<Store> GetPreconfiguredStores()
        {
            return new List<Store>()
            {
                new Store(STORE_ID, SELLER_ID, "Хорошие курсы"),
                new Store(STORE_ID+"1", SELLER_ID, "Великолепные курсы"),
                new Store(STORE_ID+"2", SELLER_ID, "Лучшие курсы"),
            };
        }
        #endregion
        #region Orders
        private static void AddPrecongifuredOrdersToFirstUser(MagShopContext context)
        {
            var user = context.Users.Find(USER_ID);
            var products = context.Products.Where(x => x.StoreId == STORE_ID);
            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (var product in products)
            {
                orderItems.Add(new OrderItem(2, product));
            }
            user.AddOrder(
                new Order(DateTime.Now, user.Addresses.First().AddressId, orderItems));
        }
        #endregion
        #region Users
        private static IEnumerable<User> GetPreconfiguredUsers(string id, string id1)
        {
            return new List<User>()
            {
                new User(id,"Дмитрий","Очеретный", "philipskryt2@gmail.ru", "+79026536953", new DateTime(1998,9,24),
                new Basket(){ BasketId=BASKET_ID},
                new List<CreditCard>(){
                    new CreditCard(){ CreditCardId=CREDIT_CARD_ID, CardNumber="12345678"}
                },
                new List<Address>()
                {
                    new Address("Pkorova", "23")
                }),

                new User(id1,"Bariga","Palenkov", "bar.yaga@mail.ru", "+7988888888", new DateTime(2005,4,27),
                new Basket(){ BasketId=BASKET_SELLER_ID},
                new List<CreditCard>(){
                    new CreditCard(){ CreditCardId=CREDIT_CARD_SELLER_ID, CardNumber="12345678"}
                },
                new List<Address>()
                {
                    new Address("Pkorova", "23")
                })
            };
        }
        #endregion
    }
}
