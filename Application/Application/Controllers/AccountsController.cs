using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Model;
using Application.SCMediator;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

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
                IList<Account> topics = await AccountService.GetAllAccounts();
                return Ok(topics);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        /* Gives an error when launching - Application.Controllers.AccountsController.LogIn (Application)' has more than one parameter that was specified or inferred as bound from request body. Only one param
         eter per action may be bound from body. Inspect the following parameters, and use 'FromQueryAttribute' to specify bound from query, 'FromRouteAttribute' to specify bound from route, and 'FromBodyAttribute' for parameters to be b
         ound from body:" */
       
        // [HttpGet]
        // [Route("{id:string}")]
        // public async Task<ActionResult<Account>> LogIn([FromBody] string username, [FromBody] string password)
        // {
        //     try
        //     {
        //         Account account = await AccountService.LogIn(username, password);
        //         return Ok(account);
        //     }
        //     catch (Exception e)
        //     {
        //         Console.WriteLine(e);
        //         return StatusCode(500, e.Message);
        //     }
        // }

        [HttpPost]
        public async Task<ActionResult> Register([FromBody] string account)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                ChatServiceImp serviceImp = new ChatServiceImp();
                
                await serviceImp.requestUser(account);
                
                return Created($"/{account}", account);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAccount([FromBody] Account account)
        {
            try
            {
                await AccountService.RemoveAccount(account);
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