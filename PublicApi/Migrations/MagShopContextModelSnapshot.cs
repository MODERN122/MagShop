﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PublicApi.Migrations
{
    [DbContext(typeof(MagShopContext))]
    partial class MagShopContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ApplicationCore.Entities.Address", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Apartment")
                        .HasColumnType("text");

                    b.Property<string>("City")
                        .HasColumnType("text");

                    b.Property<string>("Country")
                        .HasColumnType("text");

                    b.Property<int?>("Floor")
                        .HasColumnType("integer");

                    b.Property<string>("House")
                        .HasColumnType("text");

                    b.Property<string>("Housing")
                        .HasColumnType("text");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("boolean");

                    b.Property<string>("Region")
                        .HasColumnType("text");

                    b.Property<string>("Street")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("ZipCode")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("ApplicationCore.Entities.Basket", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Baskets");
                });

            modelBuilder.Entity("ApplicationCore.Entities.BasketItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("BasketId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<List<string>>("SelectedProductPropertyItemIds")
                        .HasColumnType("text[]");

                    b.HasKey("Id");

                    b.HasIndex("BasketId");

                    b.HasIndex("ProductId");

                    b.ToTable("BasketItems");
                });

            modelBuilder.Entity("ApplicationCore.Entities.Category", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ChangedByUserId")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ChangedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("ParentId")
                        .HasColumnType("text");

                    b.Property<DateTime>("PublicationDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double>("Weight")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("ApplicationCore.Entities.ChoosenProduct", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int[]>("Prices")
                        .HasColumnType("integer[]");

                    b.Property<string>("ProductId")
                        .HasColumnType("text");

                    b.Property<int>("Remain")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ChoosenProducts");
                });

            modelBuilder.Entity("ApplicationCore.Entities.CreditCard", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("CardNumber")
                        .HasColumnType("text");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("boolean");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("CreditCards");
                });

            modelBuilder.Entity("ApplicationCore.Entities.Order", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("AddressId")
                        .HasColumnType("text");

                    b.Property<string>("ChangedByUserId")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ChangedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("PublicationDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("ApplicationCore.Entities.OrderItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("OrderId")
                        .HasColumnType("text");

                    b.Property<string>("ProductId")
                        .HasColumnType("text");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("numeric(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("ApplicationCore.Entities.Product", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("CategoryId")
                        .HasColumnType("text");

                    b.Property<string>("ChangedByUserId")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ChangedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateEndDiscount")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int>("Discount")
                        .HasColumnType("integer");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("PriceNew")
                        .HasColumnType("text");

                    b.Property<string>("PriceOld")
                        .HasColumnType("text");

                    b.Property<DateTime>("PublicationDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double>("Rating")
                        .HasColumnType("double precision");

                    b.Property<string>("StoreId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Weight")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("StoreId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("ApplicationCore.Entities.ProductImage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Path")
                        .HasColumnType("text");

                    b.Property<string>("ProductId")
                        .HasColumnType("text");

                    b.Property<DateTime>("UploadDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductImages");
                });

            modelBuilder.Entity("ApplicationCore.Entities.ProductProperty", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ProductId")
                        .HasColumnType("text");

                    b.Property<string>("PropertyId")
                        .HasColumnType("text");

                    b.Property<DateTime>("PublicationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("PropertyId");

                    b.ToTable("ProductProperties");
                });

            modelBuilder.Entity("ApplicationCore.Entities.ProductPropertyItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Caption")
                        .HasColumnType("text");

                    b.Property<string>("ChangedByUserId")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ChangedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ImagePath")
                        .HasColumnType("text");

                    b.Property<double>("PriceNew")
                        .HasColumnType("double precision");

                    b.Property<double?>("PriceOld")
                        .HasColumnType("double precision");

                    b.Property<string>("ProductPropertyId")
                        .HasColumnType("text");

                    b.Property<string>("PropertyItemId")
                        .HasColumnType("text");

                    b.Property<DateTime>("PublicationDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ProductPropertyId");

                    b.HasIndex("PropertyItemId");

                    b.ToTable("ProductPropertyItems");
                });

            modelBuilder.Entity("ApplicationCore.Entities.Property", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ChangedByUserId")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ChangedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<DateTime>("PublicationDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Properties");
                });

            modelBuilder.Entity("ApplicationCore.Entities.PropertyItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Caption")
                        .HasColumnType("text");

                    b.Property<string>("PropertyId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("PropertyId");

                    b.ToTable("PropertyItems");
                });

            modelBuilder.Entity("ApplicationCore.Entities.PropertyItemTuple", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ChoosenProductId")
                        .HasColumnType("text");

                    b.Property<string>("PropertyId")
                        .HasColumnType("text");

                    b.Property<string>("PropertyItemId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ChoosenProductId");

                    b.ToTable("PropertyItemTuple");
                });

            modelBuilder.Entity("ApplicationCore.Entities.Store", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ApproveDocument")
                        .HasColumnType("text");

                    b.Property<string>("ChangedByUserId")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ChangedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<DateTime>("PublicationDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<float>("Rating")
                        .HasColumnType("real");

                    b.Property<string>("SellerId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("SellerId");

                    b.ToTable("Stores");
                });

            modelBuilder.Entity("ApplicationCore.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ChangedByUserId")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ChangedDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<List<string>>("FavoriteProductsId")
                        .HasColumnType("text[]");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<DateTime>("PublicationDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Infrastructure.Identity.UserAuthAccess", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FacebookToken")
                        .HasColumnType("text");

                    b.Property<string>("FirebaseToken")
                        .HasColumnType("text");

                    b.Property<string>("GoogleToken")
                        .HasColumnType("text");

                    b.Property<DateTime>("LastDatetimeAuth")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("OauthToken")
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("ApplicationCore.Entities.Address", b =>
                {
                    b.HasOne("ApplicationCore.Entities.User", null)
                        .WithMany("Addresses")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("ApplicationCore.Entities.Basket", b =>
                {
                    b.HasOne("ApplicationCore.Entities.User", null)
                        .WithOne("Basket")
                        .HasForeignKey("ApplicationCore.Entities.Basket", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ApplicationCore.Entities.BasketItem", b =>
                {
                    b.HasOne("ApplicationCore.Entities.Basket", null)
                        .WithMany("Items")
                        .HasForeignKey("BasketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ApplicationCore.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("ApplicationCore.Entities.Category", b =>
                {
                    b.HasOne("ApplicationCore.Entities.Category", "ParentCategory")
                        .WithMany("Childs")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("ParentCategory");
                });

            modelBuilder.Entity("ApplicationCore.Entities.ChoosenProduct", b =>
                {
                    b.HasOne("ApplicationCore.Entities.Product", "Product")
                        .WithMany("ChoosenProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Product");
                });

            modelBuilder.Entity("ApplicationCore.Entities.CreditCard", b =>
                {
                    b.HasOne("ApplicationCore.Entities.User", null)
                        .WithMany("CreditCards")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("ApplicationCore.Entities.Order", b =>
                {
                    b.HasOne("ApplicationCore.Entities.Address", "ShipToAddress")
                        .WithMany()
                        .HasForeignKey("AddressId");

                    b.HasOne("ApplicationCore.Entities.User", null)
                        .WithMany("Orders")
                        .HasForeignKey("UserId");

                    b.Navigation("ShipToAddress");
                });

            modelBuilder.Entity("ApplicationCore.Entities.OrderItem", b =>
                {
                    b.HasOne("ApplicationCore.Entities.Order", null)
                        .WithMany("Items")
                        .HasForeignKey("OrderId");

                    b.HasOne("ApplicationCore.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("ApplicationCore.Entities.Product", b =>
                {
                    b.HasOne("ApplicationCore.Entities.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId");

                    b.HasOne("ApplicationCore.Entities.Store", "Store")
                        .WithMany("StoreProducts")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("ApplicationCore.Entities.ProductImage", b =>
                {
                    b.HasOne("ApplicationCore.Entities.Product", null)
                        .WithMany("Images")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ApplicationCore.Entities.ProductProperty", b =>
                {
                    b.HasOne("ApplicationCore.Entities.Product", "Product")
                        .WithMany("ProductProperties")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ApplicationCore.Entities.Property", "Property")
                        .WithMany("ProductProperties")
                        .HasForeignKey("PropertyId");

                    b.Navigation("Product");

                    b.Navigation("Property");
                });

            modelBuilder.Entity("ApplicationCore.Entities.ProductPropertyItem", b =>
                {
                    b.HasOne("ApplicationCore.Entities.ProductProperty", "ProductProperty")
                        .WithMany("ProductPropertyItems")
                        .HasForeignKey("ProductPropertyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ApplicationCore.Entities.PropertyItem", "PropertyItem")
                        .WithMany("ProductPropertyItems")
                        .HasForeignKey("PropertyItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("ProductProperty");

                    b.Navigation("PropertyItem");
                });

            modelBuilder.Entity("ApplicationCore.Entities.PropertyItem", b =>
                {
                    b.HasOne("ApplicationCore.Entities.Property", null)
                        .WithMany("Items")
                        .HasForeignKey("PropertyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ApplicationCore.Entities.PropertyItemTuple", b =>
                {
                    b.HasOne("ApplicationCore.Entities.ChoosenProduct", null)
                        .WithMany("PropertyItemTuples")
                        .HasForeignKey("ChoosenProductId");
                });

            modelBuilder.Entity("ApplicationCore.Entities.Store", b =>
                {
                    b.HasOne("ApplicationCore.Entities.User", "Seller")
                        .WithMany("Stores")
                        .HasForeignKey("SellerId")
                        .OnDelete(DeleteBehavior.ClientCascade);

                    b.Navigation("Seller");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Infrastructure.Identity.UserAuthAccess", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Infrastructure.Identity.UserAuthAccess", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Infrastructure.Identity.UserAuthAccess", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Infrastructure.Identity.UserAuthAccess", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ApplicationCore.Entities.Basket", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("ApplicationCore.Entities.Category", b =>
                {
                    b.Navigation("Childs");

                    b.Navigation("Products");
                });

            modelBuilder.Entity("ApplicationCore.Entities.ChoosenProduct", b =>
                {
                    b.Navigation("PropertyItemTuples");
                });

            modelBuilder.Entity("ApplicationCore.Entities.Order", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("ApplicationCore.Entities.Product", b =>
                {
                    b.Navigation("ChoosenProducts");

                    b.Navigation("Images");

                    b.Navigation("ProductProperties");
                });

            modelBuilder.Entity("ApplicationCore.Entities.ProductProperty", b =>
                {
                    b.Navigation("ProductPropertyItems");
                });

            modelBuilder.Entity("ApplicationCore.Entities.Property", b =>
                {
                    b.Navigation("Items");

                    b.Navigation("ProductProperties");
                });

            modelBuilder.Entity("ApplicationCore.Entities.PropertyItem", b =>
                {
                    b.Navigation("ProductPropertyItems");
                });

            modelBuilder.Entity("ApplicationCore.Entities.Store", b =>
                {
                    b.Navigation("StoreProducts");
                });

            modelBuilder.Entity("ApplicationCore.Entities.User", b =>
                {
                    b.Navigation("Addresses");

                    b.Navigation("Basket");

                    b.Navigation("CreditCards");

                    b.Navigation("Orders");

                    b.Navigation("Stores");
                });
#pragma warning restore 612, 618
        }
    }
}
