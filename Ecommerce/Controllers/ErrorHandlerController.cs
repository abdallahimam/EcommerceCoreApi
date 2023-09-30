using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecommerce.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [ApiController]
    [Route("/errors/{code}")]
    public class ErrorHandlerController : BaseApiController
    {
        [HttpGet("{id}")]
        public IActionResult Error(int code) {
            return new ObjectResult(new ApiResponseError(code));
        }
    }
}