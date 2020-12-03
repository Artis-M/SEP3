using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Model;
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

        [HttpGet]
        [Route("{id:string}")]
        public async Task<ActionResult<Account>> LogIn([FromBody] string username, [FromBody] string password)
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
        }

        [HttpPost]
        [Route("Add")]
        public async Task<ActionResult> Register([FromBody] Account account)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await AccountService.Register(account);
                return Created($"/{account.Id}", account);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete]
        [Route("{id:string}")]
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