using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Models;
using Application.SCMediator;
using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : ControllerBase
    {
        private IAccountService AccountService;

        public AccountsController(IAccountService accountService)
        {
            this.AccountService = accountService;
        }

        [HttpGet]
        public async Task<ActionResult<IList<Account>>> GetAllAccounts()
        {
            try
            {
                
                IList<Account> accounts = await AccountService.GetAllAccounts();
                return Ok(accounts);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
        [HttpGet]
        [Route("user/{id}")]
        public async Task<ActionResult<Account>> GetUserById([FromRoute] string userID)
        {
            try
            {
                await AccountService.RequestAccounts();
                Account account = await AccountService.RequestAccountById(userID);
                return Ok(account);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet]
        [Route("login")]
        public async Task<ActionResult<Account>> GetAccount()
        {
            // try
            // {
            //     Account account = await AccountService.RequestAccount(username);
            //     IList<Account> accounts = new List<Account>();
            //     accounts.Add(account);
            //     return Ok(accounts);
            // }
            // catch (Exception e)
            // {
            //     Console.WriteLine(e);
            //     return StatusCode(500, e.Message);
            var re = this.Request;
            var headers = re.Headers;
            string username = "";
            string password = "";
            try
            {
                username = headers.GetCommaSeparatedValues("username").First();
                password = headers.GetCommaSeparatedValues("password").First();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
            Account account = await AccountService.RequestAccount(username);
            if (account == null)
            {
                return NotFound();
            }

            Console.Out.WriteLine(account.Pass);
            Console.Out.WriteLine(password);
            Console.Out.WriteLine(account.Pass == password);
            if (account.Pass != password)
            {
                return NotFound();
            }

            return Ok(account);
        }


        /* Gives an error when launching - Application.Controllers.AccountsController.LogIn (Application)' has more than one parameter that was specified or inferred as bound from request body. Only one param
         eter per action may be bound from body. Inspect the following parameters, and use 'FromQueryAttribute' to specify bound from query, 'FromRouteAttribute' to specify bound from route, and 'FromBodyAttribute' for parameters to be b
         ound from body:" */

        /*[HttpGet]
        [Route("{username, password}")]
         public async Task<ActionResult<Account>> LogIn([FromRoute] string username, [FromRoute] string password)
         {
             try
             {
                 Account account = await AccountService.LogIn(username, password);
                 return Ok(account);
             }
             catch (Exception e)
             {
                 Console.WriteLine(e);
                 return StatusCode(500, e.Message);
             }
         }*/

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register([FromBody] Account account)
        {
            
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Bad Object");
                return BadRequest(ModelState);
            }

            try
            {
                Console.Out.WriteLine(account.email);
                await AccountService.Register(account);
                return Created($"/{account._id}", account);
            }
            catch (Exception e)
            {
                Console.WriteLine("wat");
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteAccount([FromRoute] string accountID)
        {
            try
            {
                await AccountService.RemoveAccount(accountID);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
    }
}