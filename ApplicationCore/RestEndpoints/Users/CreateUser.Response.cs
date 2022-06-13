using ApplicationCore.Entities;
using ApplicationCore.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ApplicationCore.GraphQLEndpoints;

namespace ApplicationCore.RESTApi.Users
{
    public class CreateUserResponse : BaseResponse
    {
        public CreateUserResponse(Guid guid) : base(guid)
        {

        }
        public Token Token { get; set; }
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
    //
    // Сводка:
    //     Encapsulates an error from the identity subsystem.
    public class IdentityError
    {
        public IdentityError()
        {

        }

        //
        // Сводка:
        //     Gets or sets the code for this error.
        //
        // Значение:
        //     The code for this error.
        public string Code { get; set; }
        //
        // Сводка:
        //     Gets or sets the description for this error.
        //
        // Значение:
        //     The description for this error.
        public string Description { get; set; }
    }
}