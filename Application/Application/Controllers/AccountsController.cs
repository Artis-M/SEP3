using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Models;
using Application.Services;
using Microsoft.AspNetCore.Http;
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
                Account account = await AccountService.RequestAccount(username);
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
                await AccountService.Register(account);
                return Created($"/{account._id}", account);
            }
            catch (Exception e)
            {
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
        public async Task<ActionResult> RemoveFriend([FromRoute] string userId, [FromBody] string friendId)
        {
            try
            {
                await AccountService.RemoveFriend(userId, friendId);
                
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
        public async Task<ActionResult> AddTopicToUser([FromBody] string topic, [FromRoute] string userId)
        {
            try
            {
                Console.Out.WriteLine("topic:" + topic + " UserId" + "  " + userId);
                await AccountService.AddTopicToUser(userId, topic);
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
        public async Task<ActionResult> RemoveTopicFromUser([FromRoute] string topic, [FromRoute] string userId)
        {
            try
            {
                await AccountService.RemoveTopicFromUser(userId, topic);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}