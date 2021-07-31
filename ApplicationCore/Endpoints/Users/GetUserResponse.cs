using ApplicationCore.Base;
using ApplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApplicationCore.RESTApi.Users
{
    public class GetUserResponse : BaseResponse
    {
        public GetUserResponse(Guid guid) : base(guid)
        {

        }
        public IEnumerable<IdentityError> Errors { get; set; }
        public User User { get; set; }
    }
}
