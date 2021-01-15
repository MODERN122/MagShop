using ApplicationCore.Entities;
using Microsoft.AspNetCore.Identity;
using PublicApi.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PublicApi.Endpoints.Users
{
    public class CreateUserResponse : BaseResponse
    {
        public CreateUserResponse(Guid guid) : base(guid)
        {

        }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public List<CreditCard> CreditCards { get; set; }
        public List<Address> Addresses { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        public string BasketId { get; set; }
        public IEnumerable<IdentityError> Errors { get; internal set; }
    }
}