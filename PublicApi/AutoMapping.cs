using ApplicationCore.Entities;
using AutoMapper;
using PublicApi.Endpoints.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace PublicApi
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<CreateProductRequest, Product>();
            //CreateMap<Product, CreateProductResponse>();

            CreateMap<Product, GetProductsResponse>();

            CreateMap<PutProductRequest, Product>(MemberList.Source)
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>src.PriceOld!=0 && srcMember != null));
            //CreateMap<Product, PutProductResponse>();
        }
    }
}
