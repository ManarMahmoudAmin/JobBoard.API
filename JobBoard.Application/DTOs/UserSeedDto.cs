using JobBoard.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.DTOs
{
    public class UserSeedDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public UserType User_Type { get; set; }
    }
}
