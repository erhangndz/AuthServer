using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MiniApp1.API.Controllers
{
   
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {

        [Authorize(Roles = "Admin", Policy ="AnkaraPolicy")]
        [HttpGet]
        public IActionResult GetStock()
        {
            var userName = HttpContext.User.Identity.Name;

            var userIdClaim= User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);



            return Ok($"Stok İşlemleri=> UserName: {userName} - UserId: {userIdClaim.Value}");

        }
    }
}
