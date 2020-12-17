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
    /// <summary>
    /// Controller class to publish and receive data from Tier 1 regarding users/accounts
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : ControllerBase
    {
        private IAccountService AccountService;

        public AccountsController(IAccountService accountService)
        {
            this.AccountService = accountService;
        }

        /// <summary>
        /// Publishing all the accounts present
        /// </summary>
        /// <returns>List of accounts in the system</returns>
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

        /// <summary>
        /// Sending a user that has a matching ID to the given one
        /// </summary>
        /// <param name="userID">ID of User requested and published</param>
        /// <returns>Account with corresponding ID</returns>
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

        /// <summary>
        /// Getting a user by its username
        /// </summary>
        /// <param name="username">Username of requested user</param>
        /// <returns>Account that has the matching username</returns>
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

        /// <summary>
        /// Logging in ->getting the account info of a user
        /// </summary>
        /// <returns>An account that has matching password and username</returns>
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

        /// <summary>
        /// Registering a user ->Adding it to the system
        /// </summary>
        /// <param name="account">Account to be registered</param>
        /// <returns>Action result</returns>
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

        /// <summary>
        /// Deleting an account
        /// </summary>
        /// <param name="accountID">ID of account to be deleted</param>
        /// <returns>Action result</returns>
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

        /// <summary>
        /// Adding a friend
        /// </summary>
        /// <param name="users">List of 2 users to be added as each others friends</param>
        /// <returns>Action result</returns>
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

        /// <summary>
        /// Removing a friend from each other's friends list
        /// </summary>
        /// <param name="userId">Friend 1</param>
        /// <param name="friendId">Friend 2</param>
        /// <returns>Action result</returns>
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

        /// <summary>
        /// Editing an account
        /// </summary>
        /// <param name="account">New account information</param>
        /// <returns>Action result</returns>
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

        /// <summary>
        /// Adding a topic to a user
        /// </summary>
        /// <param name="topic">Topic to be added</param>
        /// <param name="userId">ID of user the topic should be added to</param>
        /// <returns>Action result</returns>
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

        /// <summary>
        /// Removing a topic from a user
        /// </summary>
        /// <param name="topic">Topic to be removed</param>
        /// <param name="userId">ID of user the topic should be removed from</param>
        /// <returns>Action result</returns>
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