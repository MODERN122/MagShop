using ApplicationCore.Entities;
using AutoMapper;
using ApplicationCore.RESTApi.Products;
using ApplicationCore.RESTApi.Users;
using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.GraphQLEndpoints;

namespace ApplicationCore
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<CreateProductRequest, Product>();

            CreateMap<Product, GetProductsResponse>();

            CreateMap<PutProductRequest, Product>(MemberList.Source)
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<CreateUserRequest, User>();
            CreateMap<User, CreateUserResponse>();

            CreateMap<EditUserInput, User>(MemberList.Source)
                .ForMember(nameof(User.BirthDate), x => x.MapFrom(y => (DateTime?)(y.BirthDate != null ? y.BirthDate.Value.DateTime.ToUniversalTime() : null)))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<User, EditUserInput>(MemberList.Source)
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Address, Address>(MemberList.Source)
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Address, Address>(MemberList.Source)
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


            CreateMap<PutProductRequest, CreateProductRequest>();
        }
    }
}
