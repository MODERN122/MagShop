using ApplicationCore.Entities;
using AutoMapper;
using ApplicationCore.Endpoints.Products;
using ApplicationCore.Endpoints.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<CreateProductRequest, Product>();
            //CreateMap<Product, CreateProductResponse>();

            CreateMap<Product, GetProductsResponse>();

            CreateMap<PutProductRequest, Product>(MemberList.Source)
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>srcMember != null));

            CreateMap<CreateUserRequest, User>();
            CreateMap<User, CreateUserResponse>();


            CreateMap<PutProductRequest, CreateProductRequest>();
            //CreateMap<Product, PutProductResponse>();
        }
    }
}
