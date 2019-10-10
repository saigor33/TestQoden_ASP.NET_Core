using System.Threading.Tasks;
using System.Web;

using Microsoft.AspNetCore.Mvc;


namespace WebApp
{
    [Route("api")]
    public class LoginController : Controller
    {
        private readonly IAccountDatabase _db;

        public LoginController(IAccountDatabase db)
        {
            _db = db;
        }

        [HttpPost("sign-in")]
        [HttpGet]
        public async Task Login(string userName)
        {
            var account = await _db.FindByUserNameAsync(userName);
            if (account != null)
            {
                //TODO 1: Generate auth cookie for user 'userName' with external id
                Response.Cookies.Append("externalId", account.InternalId.ToString());
            }
            else
            {
                //TODO 2: return 404 if user not found
                HttpContext.Response.StatusCode = 404;


            }
        }
    }
}