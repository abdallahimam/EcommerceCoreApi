using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Dtos
{
    public class UserDto
    {
        
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Token { get; set; }
    }
}