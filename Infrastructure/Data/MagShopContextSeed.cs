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
        //Properties
        private const string PROPERTY_1_ID = "property1";
        private const string PROPERTY_2_ID = "property2";
        private const string PROPERTY_3_ID = "property3";

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

                if (!await context.Properties.AnyAsync())
                {
                    await context.Properties.AddRangeAsync(
                       GetPreconfiguredProperties());
                    await context.SaveChangesAsync();
                }

                if (!await context.Products.AnyAsync())
                {
                    await context.Products.AddRangeAsync(
                        GetPrecongifuredProducts());
                    await context.SaveChangesAsync();
                    await context.Products.AddAsync(new Product("Биология", CATEGORY_3_ID, "Обучит науке о живых организмах", STORE_ID)
                    {
                        ProductProperties = new List<ProductProperty>()
                    {
                        new ProductProperty()
                        {
                            PropertyId = PROPERTY_1_ID,
                            ProductPropertyItems = GetPreconfiguredProperties(PROPERTY_1_ID).First()
                                .Items.Select(y => (y.Id, y.Caption))
                                .Select(x => new ProductPropertyItem(x.Id, x.Caption)).ToList()
                        }
                    },
                        Url = "ewqeqe"
                    });
                    await context.Products.AddAsync(new Product("Биология", CATEGORY_3_ID, "Обучит науке о живых организмах", STORE_ID)
                    {
                        ProductProperties = new List<ProductProperty>()
                    {
                        new ProductProperty()
                        {
                            PropertyId = PROPERTY_1_ID,
                            ProductPropertyItems = GetPreconfiguredProperties(PROPERTY_1_ID).First()
                                .Items.Select(y => (y.Id, y.Caption))
                                .Select(x => new ProductPropertyItem(x.Id, x.Caption)).ToList()
                        }
                    },
                        Url = "ewqeqe"
                    });
                    await context.Products.AddAsync(new Product("Биология", CATEGORY_3_ID, "Обучит науке о живых организмах", STORE_ID)
                    {
                        ProductProperties = new List<ProductProperty>()
                    {
                        new ProductProperty()
                        {
                            PropertyId = PROPERTY_1_ID,
                            ProductPropertyItems = GetPreconfiguredProperties(PROPERTY_1_ID).First()
                                .Items.Select(y => (y.Id, y.Caption))
                                .Select(x => new ProductPropertyItem(x.Id, x.Caption)).ToList()
                        }
                    },
                        Url = "ewqeqe"
                    });
                    await context.Products.AddAsync(new Product("Биология", CATEGORY_3_ID, "Обучит науке о живых организмах", STORE_ID)
                    {
                        ProductProperties = new List<ProductProperty>()
                    {
                        new ProductProperty()
                        {
                            PropertyId = PROPERTY_1_ID,
                            ProductPropertyItems = GetPreconfiguredProperties(PROPERTY_1_ID).First()
                                .Items.Select(y => (y.Id, y.Caption))
                                .Select(x => new ProductPropertyItem(x.Id, x.Caption)).ToList()
                        }
                    },
                        Url = "ewqeqe"
                    });
                    await context.Products.AddAsync(new Product("Биология", CATEGORY_3_ID, "Обучит науке о живых организмах", STORE_ID)
                    {
                        ProductProperties = new List<ProductProperty>()
                    {
                        new ProductProperty()
                        {
                            PropertyId = PROPERTY_1_ID,
                            ProductPropertyItems = GetPreconfiguredProperties(PROPERTY_1_ID).First()
                                .Items.Select(y => (y.Id, y.Caption))
                                .Select(x => new ProductPropertyItem(x.Id, x.Caption)).ToList()
                        }
                    },
                        Url = "ewqeqe"
                    });
                    await context.Products.AddAsync(new Product("Биология", CATEGORY_3_ID, "Обучит науке о живых организмах", STORE_ID)
                    {
                        ProductProperties = new List<ProductProperty>()
                    {
                        new ProductProperty()
                        {
                            PropertyId = PROPERTY_1_ID,
                            ProductPropertyItems = GetPreconfiguredProperties(PROPERTY_1_ID).First()
                                .Items.Select(y => (y.Id, y.Caption))
                                .Select(x => new ProductPropertyItem(x.Id, x.Caption)).ToList()
                        }
                    },
                        Url = "ewqeqe"
                    });
                    await context.Products.AddAsync(new Product("Биология", CATEGORY_3_ID, "Обучит науке о живых организмах", STORE_ID)
                    {
                        ProductProperties = new List<ProductProperty>()
                    {
                        new ProductProperty()
                        {
                            PropertyId = PROPERTY_1_ID,
                            ProductPropertyItems = GetPreconfiguredProperties(PROPERTY_1_ID).First()
                                .Items.Select(y => (y.Id, y.Caption))
                                .Select(x => new ProductPropertyItem(x.Id, x.Caption)).ToList()
                        }
                    },
                        Url = "ewqeqe"
                    });
                    await context.Products.AddAsync(new Product("Биология", CATEGORY_3_ID, "Обучит науке о живых организмах", STORE_ID)
                    {
                        ProductProperties = new List<ProductProperty>()
                    {
                        new ProductProperty()
                        {
                            PropertyId = PROPERTY_1_ID,
                            ProductPropertyItems = GetPreconfiguredProperties(PROPERTY_1_ID).First()
                                .Items.Select(y => (y.Id, y.Caption))
                                .Select(x => new ProductPropertyItem(x.Id, x.Caption)).ToList()
                        }
                    },
                        Url = "ewqeqe"
                    });
                    await context.Products.AddAsync(new Product("Биология", CATEGORY_3_ID, "Обучит науке о живых организмах", STORE_ID)
                    {
                        ProductProperties = new List<ProductProperty>()
                    {
                        new ProductProperty()
                        {
                            PropertyId = PROPERTY_1_ID,
                            ProductPropertyItems = GetPreconfiguredProperties(PROPERTY_1_ID).First()
                                .Items.Select(y => (y.Id, y.Caption))
                                .Select(x => new ProductPropertyItem(x.Id, x.Caption)).ToList()
                        }
                    },
                        Url = "ewqeqe"
                    });
                    await context.Products.AddAsync(new Product("Биология", CATEGORY_3_ID, "Обучит науке о живых организмах", STORE_ID)
                    {
                        ProductProperties = new List<ProductProperty>()
                    {
                        new ProductProperty()
                        {
                            PropertyId = PROPERTY_1_ID,
                            ProductPropertyItems = GetPreconfiguredProperties(PROPERTY_1_ID).First()
                                .Items.Select(y => (y.Id, y.Caption))
                                .Select(x => new ProductPropertyItem(x.Id, x.Caption)).ToList()
                        }
                    },
                        Url = "ewqeqe"
                    });
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

        private static List<Property> GetPreconfiguredProperties(string propertyId=null)
        {
            #region IMAGES
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
            #endregion
            Random random = new Random();
            var list = new List<Property>()
            {
               new Property(PROPERTY_1_ID, "Классы",
                new List<PropertyItem>(){
                    new PropertyItem("1","1"),
                    new PropertyItem("2","2"),
                    new PropertyItem("3","3"),
                    new PropertyItem("4","4"),
                    new PropertyItem("5","5"),
                    new PropertyItem("6","6"),
                    new PropertyItem("7","7"),
                    new PropertyItem("8","8"),
                    new PropertyItem("9","9"),
                }),
                new Property(PROPERTY_2_ID,"Типы",
                   new List<PropertyItem>(){
                       new PropertyItem("10","Для голубей"),
                       new PropertyItem("12","Для воробьев"),
                       new PropertyItem("13","Для белок"),
                       new PropertyItem("14","Для кошек"),

                   }),
                new Property(PROPERTY_3_ID,"Уровень сложности",
                    new List<PropertyItem>(){
                        new PropertyItem("15","Минимальный"),
                        new PropertyItem("16","Нормальный"),
                        new PropertyItem("17","Продвинутый"),
                        new PropertyItem("18","Экспертный"),
                    })
            };
            if (propertyId != null)
            {
                return list.Where(x => x.Id == propertyId).ToList();
            }
            else
            {
                return list;
            }
        }

        private static void AddPreconfiguredBasketItemsToFirstUser(MagShopContext context)
        {
            var user = context.Users.Include(x => x.Basket)
                    .ThenInclude(x => x.Items).First(x => x.Id == USER_ID);
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
            #region IMAGES
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
            #endregion
            Random random = new Random();
            Func<string, List<ProductPropertyItem>> func = x=> GetPreconfiguredProperties(x).First()
                                .Items.Select(y => (y.Id, y.Caption))
                                .Select(x => new ProductPropertyItem(x.Id, x.Caption)).ToList();
            return new List<Product>()
            {
                new Product(PRODUCT_1_ID, "Биология", CATEGORY_3_ID, "Обучит науке о живых организмах", STORE_ID)
                 {
                    ProductProperties = new List<ProductProperty>()
                    {
                        new ProductProperty()
                        {
                            PropertyId = PROPERTY_1_ID,
                            ProductPropertyItems = func(PROPERTY_1_ID)
                        }
                    },
                    Url = "ewqeqe"
                },

                new Product(PRODUCT_2_ID, "Корушка", CATEGORY_6_ID, "Сдлайте все своими руками",
                 STORE_ID)
                {
                    ProductProperties = new List<ProductProperty>()
                    {
                        new ProductProperty()
                        {
                            ProductId = PRODUCT_2_ID,
                            PropertyId = PROPERTY_2_ID,
                            ProductPropertyItems = func(PROPERTY_1_ID)
                        }
                    },
                    Url = "ewqeqe"
                },
                new Product(PRODUCT_3_ID, "1с", CATEGORY_5_ID, "Обучитесь программированию на 1с",
                STORE_ID)
                {ProductProperties = new List<ProductProperty>()
                    {
                        new ProductProperty()
                        {
                            ProductId = PRODUCT_3_ID,
                            PropertyId = PROPERTY_1_ID,
                            ProductPropertyItems = func(PROPERTY_1_ID)
                        }
                    },
                    Url = "ewqeqe"
                },
                new Product(PRODUCT_4_ID, "Физика", CATEGORY_4_ID, "Обучение основам физики"

                , STORE_ID)
                {ProductProperties = new List<ProductProperty>()
                    {
                        new ProductProperty()
                        {
                            ProductId = PRODUCT_4_ID,
                            PropertyId = PROPERTY_1_ID,
                            ProductPropertyItems = func(PROPERTY_2_ID)
                        }
                    },
                    Url = "ewqeqe"
                },
                new Product(PRODUCT_5_ID, "Французкий язык", CATEGORY_2_ID, "Научитесь говорить как истинный француз",
               STORE_ID+"1")
                {ProductProperties = new List<ProductProperty>()
                    {
                        new ProductProperty()
                        {
                            ProductId = PRODUCT_5_ID,
                            PropertyId = PROPERTY_3_ID,
                            ProductPropertyItems = func(PROPERTY_1_ID)
                        }
                    },
                    Url = "ewqeqe"
                },
                new Product(PRODUCT_6_ID, "Математика", CATEGORY_1_ID, "Научитесть складывать цифры и не только",
                STORE_ID+"1")
                {ProductProperties = new List<ProductProperty>()
                    {
                        new ProductProperty()
                        {
                            ProductId = PRODUCT_6_ID,
                            PropertyId = PROPERTY_3_ID,
                            ProductPropertyItems = func(PROPERTY_2_ID)
                        }
                    },
                    Url = "ewqeqe"
                },
                new Product(PRODUCT_7_ID, "Русский язык", CATEGORY_2_ID, "Обучат грамматике",
               STORE_ID+"2")
                {ProductProperties = new List<ProductProperty>()
                    {
                        new ProductProperty()
                        {
                            ProductId = PRODUCT_7_ID,
                            PropertyId = PROPERTY_1_ID,
                            ProductPropertyItems = func(PROPERTY_1_ID)
                        }
                    },
                    Url = "ewqeqe"
                },
                new Product(PRODUCT_8_ID, "География", CATEGORY_4_ID, "Научат ориентироваться в картах",  STORE_ID+"2")
                {ProductProperties = new List<ProductProperty>()
                    {
                        new ProductProperty()
                        {
                            ProductId = PRODUCT_8_ID,
                            PropertyId = PROPERTY_2_ID,
                            ProductPropertyItems = func(PROPERTY_3_ID)
                        }
                    },
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
                orderItems.Add(new OrderItem(2, product, product.ProductProperties.Select(x => x.ProductPropertyItems.First()).ToList()));
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
                new Basket(){ Id=BASKET_ID},
                new List<CreditCard>(){
                    new CreditCard(){ CreditCardId=CREDIT_CARD_ID, CardNumber="12345678"}
                },
                new List<Address>()
                {
                    new Address("Pkorova", "23")
                }),

                new User(id1,"Иван","Иванович", "i.i@mail.ru", "+7988888888", new DateTime(2005,4,27),
                new Basket(){ Id=BASKET_SELLER_ID},
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
