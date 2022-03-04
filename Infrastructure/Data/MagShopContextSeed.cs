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
        private const string ADMINISTRATOR_ID = "admin@microsoft.com";
        private const string USER_ID = "user@microsoft.com";
        private const string BASKET_ID = "12345";
        private const string CREDIT_CARD_ID = "123";
        //Stores
        private const string SELLER_ID = "demoseller@microsoft.com";
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
                //context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();
                context.Database.Migrate();
                if (!await context.Users.AnyAsync())
                {
                    if (!roleManager.Roles.Any() && !userManager.Users.Any())
                    {
                        await roleManager.CreateAsync(new IdentityRole(Constants.ConstantsAPI.ADMINISTRATORS));
                        await roleManager.CreateAsync(new IdentityRole(Constants.ConstantsAPI.SELLERS));
                        await roleManager.CreateAsync(new IdentityRole(Constants.ConstantsAPI.USERS));

                        var sellerUser = new UserAuthAccess(SELLER_ID);
                        await userManager.CreateAsync(sellerUser, Constants.ConstantsAPI.DEFAULT_PASSWORD);
                        var seller = await userManager.FindByNameAsync(sellerUser.UserName);
                        await userManager.AddToRoleAsync(seller, Constants.ConstantsAPI.SELLERS);

                        var user = new UserAuthAccess(USER_ID);
                        await userManager.CreateAsync(user, Constants.ConstantsAPI.DEFAULT_PASSWORD);
                        user = await userManager.FindByNameAsync(user.UserName);
                        await userManager.AddToRoleAsync(user, Constants.ConstantsAPI.USERS);

                        var adminUser = new UserAuthAccess(ADMINISTRATOR_ID);
                        await userManager.CreateAsync(adminUser, Constants.ConstantsAPI.DEFAULT_PASSWORD);
                        adminUser = await userManager.FindByNameAsync(adminUser.UserName);
                        await userManager.AddToRoleAsync(adminUser, Constants.ConstantsAPI.ADMINISTRATORS);

                        await context.Users.AddRangeAsync(
                            GetPreconfiguredUsers(seller.Id,user.Id, adminUser.Id));
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
                    await context.Products.AddAsync(new Product("22","Биология", CATEGORY_3_ID, "Обучит науке о живых организмах", STORE_ID, new List<ProductProperty>()
                    {
                        new ProductProperty()
                        {
                            PropertyId = PROPERTY_1_ID,
                            ProductPropertyItems = GetPreconfiguredProperties(PROPERTY_1_ID).First()
                                .Items
                                .Select(x => new ProductPropertyItem(x.Id, x.Caption,SELLER_ID)).ToList()
                        }
                    }, SELLER_ID)
                   
                    );
                    await context.Products.AddAsync(new Product("Биология", CATEGORY_3_ID, "Обучит науке о живых организмах", STORE_ID, new List<ProductProperty>()
                    {
                        new ProductProperty()
                        {
                            PropertyId = PROPERTY_1_ID,
                            ProductPropertyItems = GetPreconfiguredProperties(PROPERTY_1_ID).First()
                                .Items
                                .Select(x => new ProductPropertyItem(x.Id, x.Caption,SELLER_ID)).ToList()
                        }
                    }, SELLER_ID)
                    );
                    await context.Products.AddAsync(new Product("Биология", CATEGORY_3_ID, "Обучит науке о живых организмах", STORE_ID, new List<ProductProperty>()
                    {
                        new ProductProperty()
                        {
                            PropertyId = PROPERTY_1_ID,
                            ProductPropertyItems = GetPreconfiguredProperties(PROPERTY_1_ID).First()
                                .Items
                                .Select(x => new ProductPropertyItem(x.Id, x.Caption,SELLER_ID)).ToList()
                        }
                    }, SELLER_ID)); await context.Products.AddAsync(new Product("Биология", CATEGORY_3_ID, "Обучит науке о живых организмах", STORE_ID, new List<ProductProperty>()
                    {
                        new ProductProperty()
                        {
                            PropertyId = PROPERTY_1_ID,
                            ProductPropertyItems = GetPreconfiguredProperties(PROPERTY_1_ID).First()
                                .Items
                                .Select(x => new ProductPropertyItem(x.Id, x.Caption,SELLER_ID)).ToList()
                        }
                    }, SELLER_ID)
                    ); await context.Products.AddAsync(new Product("Биология", CATEGORY_3_ID, "Обучит науке о живых организмах", STORE_ID, new List<ProductProperty>()
                    {
                        new ProductProperty()
                        {
                            PropertyId = PROPERTY_1_ID,
                            ProductPropertyItems = GetPreconfiguredProperties(PROPERTY_1_ID).First()
                                .Items
                                .Select(x => new ProductPropertyItem(x.Id, x.Caption,SELLER_ID)).ToList()
                        }
                    }, SELLER_ID)
                    ); await context.Products.AddAsync(new Product("Биология", CATEGORY_3_ID, "Обучит науке о живых организмах", STORE_ID, new List<ProductProperty>()
                    {
                        new ProductProperty()
                        {
                            PropertyId = PROPERTY_1_ID,
                            ProductPropertyItems = GetPreconfiguredProperties(PROPERTY_1_ID).First()
                                .Items
                                .Select(x => new ProductPropertyItem(x.Id, x.Caption,SELLER_ID)).ToList()
                        }
                    }, SELLER_ID)
                   ); await context.Products.AddAsync(new Product("Биология", CATEGORY_3_ID, "Обучит науке о живых организмах", STORE_ID, new List<ProductProperty>()
                    {
                        new ProductProperty()
                        {
                            PropertyId = PROPERTY_1_ID,
                            ProductPropertyItems = GetPreconfiguredProperties(PROPERTY_1_ID).First()
                                .Items
                                .Select(x => new ProductPropertyItem(x.Id, x.Caption,SELLER_ID)).ToList()
                        }
                    }, SELLER_ID)
                    ); await context.Products.AddAsync(new Product("Биология", CATEGORY_3_ID, "Обучит науке о живых организмах", STORE_ID, new List<ProductProperty>()
                    {
                        new ProductProperty()
                        {
                            PropertyId = PROPERTY_1_ID,
                            ProductPropertyItems = GetPreconfiguredProperties(PROPERTY_1_ID).First()
                                .Items
                                .Select(x => new ProductPropertyItem(x.Id, x.Caption,SELLER_ID)).ToList()
                        }
                    }, SELLER_ID)
                    ); await context.Products.AddAsync(new Product("Биология", CATEGORY_3_ID, "Обучит науке о живых организмах", STORE_ID, new List<ProductProperty>()
                    {
                        new ProductProperty()
                        {
                            PropertyId = PROPERTY_1_ID,
                            ProductPropertyItems = GetPreconfiguredProperties(PROPERTY_1_ID).First()
                                .Items
                                .Select(x => new ProductPropertyItem(x.Id, x.Caption,SELLER_ID)).ToList()
                        }
                    }, SELLER_ID)
                   );
                    await context.SaveChangesAsync();
                }

                if (!await context.Orders.AnyAsync())
                {
                    AddPrecongifuredOrdersToFirstUser(context);
                    await context.SaveChangesAsync();
                }

                var users = await context.Users.Include(x => x.Basket)
                    .ThenInclude(x => x.Items).ToListAsync();

                if (!users.Any(x=>x.Basket?.Items.Count>0))
                {
                    AddPreconfiguredBasketItemsToFirstUser(context);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 1)
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<MagShopContextSeed>();
                    log.LogError(ex.Message);
                    await SeedAsync(context, loggerFactory, userManager, roleManager, retryForAvailability);
                }
                throw;
            }
        }

        private static List<Property> GetPreconfiguredProperties(string propertyId = null)
        {
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
                },SELLER_ID),
                new Property(PROPERTY_2_ID,"Типы",
                   new List<PropertyItem>(){
                       new PropertyItem("10","Для голубей"),
                       new PropertyItem("12","Для воробьев"),
                       new PropertyItem("13","Для белок"),
                       new PropertyItem("14","Для кошек"),

                   },SELLER_ID),
                new Property(PROPERTY_3_ID,"Уровень сложности",
                    new List<PropertyItem>(){
                        new PropertyItem("15","Минимальный"),
                        new PropertyItem("16","Нормальный"),
                        new PropertyItem("17","Продвинутый"),
                        new PropertyItem("18","Экспертный"),
                    },SELLER_ID)
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
                    .ThenInclude(x => x.Items).First(x => x.Id == ADMINISTRATOR_ID);
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
                new Category(PARENT_ID, null, "MAIN_CATEGORY", 0,SELLER_ID),
                new Category(CATEGORY_1_ID, PARENT_ID, "Электроника", 0.2,SELLER_ID),
                new Category(CATEGORY_2_ID, PARENT_ID, "Мужской гардероб", 0.2,SELLER_ID),
                new Category(CATEGORY_3_ID, PARENT_ID, "Здоровье", 0.1,SELLER_ID),
                new Category("category7", CATEGORY_3_ID, "Лекарства", 0.1,SELLER_ID),
                new Category("category8", CATEGORY_3_ID, "Тренажеры", 0.1,SELLER_ID),
                new Category(CATEGORY_4_ID, PARENT_ID, "Украшения", 0.2,SELLER_ID ),
                new Category(CATEGORY_5_ID, PARENT_ID, "Строительство", 0.25,SELLER_ID),
                new Category(CATEGORY_6_ID, PARENT_ID, "Товары для дома", 0.31,SELLER_ID)

            };
        }
        #endregion
        #region Products
        private static IEnumerable<Product> GetPrecongifuredProducts()
        {
            Random random = new Random();
            Func<string, List<ProductPropertyItem>> func = x => GetPreconfiguredProperties(x).First()
                                .Items.Select(y => (y.Id, y.Caption))
                                .Select(x => new ProductPropertyItem(x.Id, x.Caption, SELLER_ID)).ToList();
            return new List<Product>()
            {
                new Product(PRODUCT_1_ID, "Биология", CATEGORY_3_ID, "Обучит науке о живых организмах", STORE_ID,
                new List<ProductProperty>()
                    {
                        new ProductProperty()
                        {
                            PropertyId = PROPERTY_1_ID,
                            ProductPropertyItems = func(PROPERTY_1_ID)
                        }
                    },SELLER_ID),

                new Product(PRODUCT_2_ID, "Корушка", CATEGORY_6_ID, "Сдлайте все своими руками",
                 STORE_ID, new List<ProductProperty>()
                    {
                        new ProductProperty()
                        {
                            ProductId = PRODUCT_2_ID,
                            PropertyId = PROPERTY_2_ID,
                            ProductPropertyItems = func(PROPERTY_1_ID)
                        }
                    },SELLER_ID),
                new Product(PRODUCT_3_ID, "1с", CATEGORY_5_ID, "Обучитесь программированию на 1с",
                STORE_ID,new List<ProductProperty>()
                    {
                        new ProductProperty()
                        {
                            ProductId = PRODUCT_3_ID,
                            PropertyId = PROPERTY_1_ID,
                            ProductPropertyItems = func(PROPERTY_1_ID)
                        }
                    },SELLER_ID),
                new Product(PRODUCT_4_ID, "Физика", CATEGORY_4_ID, "Обучение основам физики"

                , STORE_ID,new List<ProductProperty>()
                    {
                        new ProductProperty()
                        {
                            ProductId = PRODUCT_4_ID,
                            PropertyId = PROPERTY_1_ID,
                            ProductPropertyItems = func(PROPERTY_2_ID)
                        }
                    },SELLER_ID),
                new Product(PRODUCT_5_ID, "Французкий язык", CATEGORY_2_ID, "Научитесь говорить как истинный француз",
               STORE_ID+"1",new List<ProductProperty>()
                    {
                        new ProductProperty()
                        {
                            ProductId = PRODUCT_5_ID,
                            PropertyId = PROPERTY_3_ID,
                            ProductPropertyItems = func(PROPERTY_1_ID)
                        }
                    },SELLER_ID),
                new Product(PRODUCT_6_ID, "Математика", CATEGORY_1_ID, "Научитесть складывать цифры и не только",
                STORE_ID+"1",new List<ProductProperty>()
                    {
                        new ProductProperty()
                        {
                            ProductId = PRODUCT_6_ID,
                            PropertyId = PROPERTY_3_ID,
                            ProductPropertyItems = func(PROPERTY_2_ID)
                        }
                    },SELLER_ID),
                new Product(PRODUCT_7_ID, "Русский язык", CATEGORY_2_ID, "Обучат грамматике",
               STORE_ID+"2", new List<ProductProperty>()
                    {
                        new ProductProperty()
                        {
                            ProductId = PRODUCT_7_ID,
                            PropertyId = PROPERTY_1_ID,
                            ProductPropertyItems = func(PROPERTY_1_ID)
                        }
                    },SELLER_ID),
                new Product(PRODUCT_8_ID, "География", CATEGORY_4_ID, "Научат ориентироваться в картах",  STORE_ID+"2", new List<ProductProperty>()
                    {
                        new ProductProperty()
                        {
                            ProductId = PRODUCT_8_ID,
                            PropertyId = PROPERTY_2_ID,
                            ProductPropertyItems = func(PROPERTY_3_ID)
                        }
                    },SELLER_ID),
            };
        }
        #endregion
        #region Stores
        private static IEnumerable<Store> GetPreconfiguredStores()
        {
            return new List<Store>()
            {
                new Store(STORE_ID, SELLER_ID, "Хорошие курсы", 3.0f),
                new Store(STORE_ID+"1", SELLER_ID, "Великолепные курсы", 4.2f),
                new Store(STORE_ID+"2", SELLER_ID, "Лучшие курсы", 4.3f),
            };
        }
        #endregion
        #region Orders
        private static void AddPrecongifuredOrdersToFirstUser(MagShopContext context)
        {
            var user = context.Users.Find(ADMINISTRATOR_ID);
            var products = context.Products.Where(x => x.StoreId == STORE_ID);
            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (var product in products)
            {
                orderItems.Add(new OrderItem(2, product, product.ProductProperties.Select(x => x.ProductPropertyItems.First()).ToList()));
            }
            context.Orders.Add(
                new Order(DateTime.UtcNow, user.Addresses.First().AddressId, orderItems, SELLER_ID));
        }
        #endregion
        #region Users
        private static IEnumerable<User> GetPreconfiguredUsers(string sellerId, string userId, string adminId)
        {
            return new List<User>()
            {
                new User(sellerId,"Дмитрий","Очеретный", "philipskryt2@gmail.ru", "+79026536953", new DateTime(1998,09,24),
                new Basket(){ Id=BASKET_ID},
                new List<CreditCard>(){
                    new CreditCard(){ CreditCardId=CREDIT_CARD_ID, CardNumber="12345678"}
                },
                new List<Address>()
                {
                    new Address("Pkorova", "23")
                }),

                new User(userId,"Иван","Иванович", "i.i@mail.ru", "+7988888888", new DateTime(2005,4,27),
                new Basket(){ Id=BASKET_SELLER_ID},
                new List<CreditCard>(){
                    new CreditCard(){ CreditCardId=CREDIT_CARD_SELLER_ID, CardNumber="12345678"}
                },
                new List<Address>()
                {
                    new Address("Pkorova", "23")
                }),
                new User(adminId,"USer","Userovich", "i.i@mail.ru", "+7988888888", new DateTime(2005,4,27),
                new Basket(){ Id=BASKET_SELLER_ID+"1"},
                new List<CreditCard>(){
                    new CreditCard(){ CreditCardId=CREDIT_CARD_SELLER_ID+"1", CardNumber="12345678"}
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
