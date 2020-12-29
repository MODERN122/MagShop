using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class MagShopContextSeed
    {
        //Users
        private const string USER_ID = "1234";
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

        private const string PROPERTY_ID = "property1";
        private const string PROPERTY_ITEM_1_ID = "propertyitem1";
        private const string PROPERTY_ITEM_2_ID = "propertyitem2";
        private const string PROPERTY_ITEM_3_ID = "propertyitem3";
        private const string PROPERTY_ITEM_4_ID = "propertyitem4";
        private const string PROPERTY_ITEM_5_ID = "propertyitem5";
        //Category
        public const string PARENT_ID = "mainCategory";
        private const string CATEGORY_1_ID = "category1";
        private const string CATEGORY_2_ID = "category2";
        private const string CATEGORY_3_ID = "category3";
        private const string CATEGORY_4_ID = "category4";
        private const string CATEGORY_5_ID = "category5";
        private const string CATEGORY_6_ID = "category6";

        public static async Task SeedAsync(MagShopContext context,
            ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retryForAvailability = retry.Value;
            try
            {
                // TODO: Only run this if using a real database
                //context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                //context.Database.Migrate();
                if (!await context.Users.AnyAsync())
                {
                    await context.Users.AddRangeAsync(
                        GetPreconfiguredUsers());

                    await context.SaveChangesAsync();
                }

                if (!await context.Orders.AnyAsync())
                {
                    await context.Orders.AddRangeAsync(
                        GetPrecongifuredOrders(context));
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
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<MagShopContextSeed>();
                    log.LogError(ex.Message);
                    await SeedAsync(context, loggerFactory, retryForAvailability);
                }
                throw;
            }
        }

        private static IEnumerable<Category> GetPreconfiguredCategories()
        {
            return new List<Category>()
            {
                new Category(PARENT_ID, null, "MAIN_CATEGORY", 0),
                new Category(CATEGORY_1_ID, PARENT_ID, "Shoes", 0.2),
                new Category(CATEGORY_2_ID, PARENT_ID, "Clothes", 0.2),
                new Category(CATEGORY_3_ID, PARENT_ID, "Backpacks", 0.1),
                new Category(CATEGORY_4_ID, CATEGORY_2_ID, "Jackets", 0.2 ),
                new Category(CATEGORY_5_ID, CATEGORY_2_ID, "T-shirts", 0.25),
                new Category(CATEGORY_6_ID, CATEGORY_2_ID, "Pants", 0.311233223)

            };
        }

        private static IEnumerable<Product> GetPrecongifuredProducts()
        {
            return new List<Product>()
            {
                new Product(PRODUCT_1_ID, "Russian backpack", 1800, CATEGORY_3_ID, "It`s nice backpack for real nice guys",
                new List<Property>()
                {
                    new Property("Size",
                    new List<PropertyItem>(){
                        new PropertyItem("41"),
                        new PropertyItem("42"),
                        new PropertyItem("43"),
                        new PropertyItem("44"),
                    })
                }, STORE_ID),
                new Product(PRODUCT_2_ID, "Jeans Versache", 1800, CATEGORY_6_ID, "It`s nice Versache for all",
                new List<Property>()
                {
                    new Property("Size",
                    new List<PropertyItem>(){
                        new PropertyItem("41"),
                        new PropertyItem("42"),
                        new PropertyItem("43"),
                        new PropertyItem("44"),
                    })
                }, STORE_ID),
                new Product(PRODUCT_3_ID, "Crunch backpack", 1800, CATEGORY_3_ID, "It`s nice backpack for real nice guys",
                new List<Property>()
                {
                    new Property("Size",
                    new List<PropertyItem>(){
                        new PropertyItem("41"),
                        new PropertyItem("42"),
                        new PropertyItem("43"),
                        new PropertyItem("44"),
                    })
                }, STORE_ID),
                new Product(PRODUCT_4_ID, "French backpack", 1800, CATEGORY_3_ID, "It`s nice backpack for real nice guys",
                new List<Property>()
                {
                    new Property("Size",
                    new List<PropertyItem>(){
                        new PropertyItem("41"),
                        new PropertyItem("42"),
                        new PropertyItem("43"),
                        new PropertyItem("44"),
                    })
                }, STORE_ID),
                new Product(PRODUCT_5_ID, "Belorussian pack", 1800, CATEGORY_3_ID, "It`s nice backpack for real nice guys",
                new List<Property>()
                {
                    new Property("Size",
                    new List<PropertyItem>(){
                        new PropertyItem("41"),
                        new PropertyItem("42"),
                        new PropertyItem("43"),
                        new PropertyItem("44"),
                    })
                }, STORE_ID),
                new Product(PRODUCT_6_ID, "Russian shoe", 1800, CATEGORY_1_ID, "It`s nice shoe for real nice girls",
                new List<Property>()
                {
                    new Property("Size",
                    new List<PropertyItem>(){
                        new PropertyItem("41"),
                        new PropertyItem("42"),
                        new PropertyItem("43"),
                        new PropertyItem("44"),
                    })
                }, STORE_ID),
                new Product(PRODUCT_7_ID, "Best jacket", 1800, CATEGORY_4_ID, "It`s nice cardigan for real nice guys",
                new List<Property>()
                {
                    new Property("Size",
                    new List<PropertyItem>(){
                        new PropertyItem("41"),
                        new PropertyItem("42"),
                        new PropertyItem("43"),
                        new PropertyItem("44"),
                    })
                }, STORE_ID),
                new Product(PRODUCT_8_ID, "French cardigan", 18000, CATEGORY_4_ID, "It`s nice cardigan for real nice guys",
                new List<Property>()
                {
                    new Property("Size",
                    new List<PropertyItem>(){
                        new PropertyItem("41"),
                        new PropertyItem("42"),
                        new PropertyItem("43"),
                        new PropertyItem("44"),
                    })
                }, STORE_ID),
            };
        }

        private static IEnumerable<Store> GetPreconfiguredStores()
        {
            return new List<Store>()
            {
                new Store(STORE_ID, SELLER_ID, "NICE STORE"),
                new Store(STORE_ID+"1", SELLER_ID, "GOOD STORE"),
                new Store(STORE_ID+"2", SELLER_ID, "BEST STORE"),
            };
        }

        private static IEnumerable<Order> GetPrecongifuredOrders(MagShopContext context)
        {
            var user = context.Users.Find(USER_ID);
            return new List<Order>();
        }

        private static IEnumerable<User> GetPreconfiguredUsers()
        {
            return new List<User>()
            {
                new User(USER_ID,"Mikhail","Filippov", "filmih24@mail.ru", "+79954116858", DateTimeOffset.Parse("24.09.1998"),
                new Basket(){ BasketId=BASKET_ID}, new UserAuthAccess("admin","password"),
                new List<CreditCard>(){
                    new CreditCard(){ CreditCardId=CREDIT_CARD_ID, CardNumber="12345678"}
                },
                new List<Address>()
                {
                    new Address("Pkorova", "23")
                }),

                new User(SELLER_ID,"Bariga","Palenkov", "bar.yaga@mail.ru", "+7988888888", DateTimeOffset.Parse("21.7.2005"),
                new Basket(){ BasketId=BASKET_SELLER_ID}, new UserAuthAccess("admin","password"),
                new List<CreditCard>(){
                    new CreditCard(){ CreditCardId=CREDIT_CARD_SELLER_ID, CardNumber="12345678"}
                },
                new List<Address>()
                {
                    new Address("Pkorova", "23")
                })
            };
        }
    }
}
