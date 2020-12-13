using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        [Route("user/{userID}")]
        public async Task<ActionResult<Account>> GetUserById([FromRoute] string userID)
        {
            try
            {
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
        [Route("user/username/{username}")]
        public async Task<ActionResult<Account>> GetUserByUsername([FromRoute] string username)
        {
            try
            {
                Console.Out.WriteLine(username);
                Account account = await AccountService.RequestAccount(username);
                Console.Out.WriteLine("username");
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

           // Console.Out.WriteLine(account.Pass);
           // Console.Out.WriteLine(password);
           // Console.Out.WriteLine(account.Pass == password);
            if (account.Pass != password)
            {
                return NotFound();
            }

            return Ok(account);
        }

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
        [Route("{accountID}")]
        public async Task<ActionResult> DeleteAccount([FromRoute] string accountID)
        {
            try
            {
                await AccountService.RemoveAccount(accountID);
                Console.Out.WriteLine(accountID+"DELETE ME");
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpPatch]
        [Route("addFriend")]
        public async Task<ActionResult> AddFriend([FromBody] List<User> users)
        {
            Console.WriteLine($"{users[0]}, {users[1]}");
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Bad Object");
                return BadRequest(ModelState);
            }

            try
            {
                await AccountService.AddFriend(users);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
        [HttpPatch]
        [Route("user/removeFriend/{userId}")]
        public async Task<ActionResult> removeFriend([FromRoute] string userId, [FromBody] string friendId)
        {
            try
            {
                await AccountService.removeFriend(userId, friendId);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        [HttpPatch]
        [Route("editAccount")]
        public async Task<ActionResult> EditAccount([FromBody] Account account)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Bad Object");
                return BadRequest(ModelState);
            }

            try
            {
                await AccountService.EditAccount(account);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
        [HttpPost]
        [Route("topic/add/{userId}")]
        public async Task<ActionResult> addTopicToUser([FromBody] string topic, [FromRoute] string userId)
        {
            Console.Out.WriteLine();
            try
            {
                Console.Out.WriteLine("topic:" + topic + " UserId" + "  " + userId);
                await AccountService.addTopicToUser(userId, topic);
                Console.Out.WriteLine("Topic added");
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        [HttpDelete]
        [Route("topic/remove/{userId}/{topic}")]
        public async Task<ActionResult> removeTopicFromUser([FromRoute] string topic, [FromRoute] string userId)
        {
            try
            {
                await AccountService.removeTopicFromUser(userId, topic);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}