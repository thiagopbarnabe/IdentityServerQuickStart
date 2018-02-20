using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MyApi.Controllers
{
    public class IdentityController : Controller
    {
        [Route("identity")]        
        [Authorize()]
        [HttpGet]
        public IActionResult Index()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}