using ApplicationCore.Base;
using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApplicationCore.Endpoints.Users
{
    public class GetUserResponse : BaseResponse
    {
        public GetUserResponse(Guid guid) : base(guid)
        {

        }
        public string Token { get; set; }
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
        public IEnumerable<IdentityError> Errors { get; set; }
    }
}
