using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp
{
    //+ TODO 5: unauthorized users should receive 401 status code
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Authorize]
        [HttpGet]
        public ValueTask<Account> Get()
        {
            /// _accountService.
            long externalId;
            if(long.TryParse(Request.Cookies["externalId"], out externalId))
            {
                return _accountService.LoadOrCreateAsync(externalId); //null /*+ TODO 3: Get user id from cookie */
            }
            else
            {
                Response.StatusCode = 401;
                return _accountService.LoadOrCreateAsync(null);
            }
        }

        //+TODO 6: Endpoint should works only for users with "Admin" Role
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public Account GetByInternalId([FromRoute] int id)
        {
            return _accountService.GetFromCache(id);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("counter")]
        public async Task UpdateAccount()
        {
            //Update account in cache, don't bother saving to DB, this is not an objective of this task.
            var account = await Get();
            account.Counter++;
        }
    }
}