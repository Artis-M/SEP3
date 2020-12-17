using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Models;
using Application.SCMediator;

namespace Application.Services
{
    /// <summary>
    /// Class that is used to handle necessary methods that are later used in the Controller classes
    /// </summary>
    public class AccountsServiceImpl : IAccountService
    {
        public List<Account> Accounts { get; set; }
        public Model model;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="modelManager">model to be initialized</param>
        public AccountsServiceImpl(Model modelManager)
        {
            this.model = modelManager;
            Accounts = new List<Account>();
        }

        /// <summary>
        /// Registering a new Account
        /// </summary>
        /// <param name="account">Account to be registered</param>
        /// <returns>no return</returns>
        public async Task Register(Account account)
        {
            this.Accounts.Add(account);
            await model.Register(account);
        }

        /// <summary>
        /// Removing account from the system
        /// </summary>
        /// <param name="accountID">Account's ID that should be removed</param>
        /// <returns>no return</returns>
        public async Task RemoveAccount(string accountID)
        {
            await model.RemoveUser(accountID);
        }

        /// <summary>
        /// Requesting all accounts present in the system and overwriting the existing list with the recieved ones
        /// </summary>
        /// <returns>no return</returns>
        public async Task RequestAccounts()
        {
            Accounts = await model.RequestUsers();
        }

        /// <summary>
        /// Method to call RequestAccounts and return the recieved ones
        /// </summary>
        /// <returns>List of accounts in the system</returns>
        public async Task<IList<Account>> GetAllAccounts()
        {
            await RequestAccounts();
            return Accounts;
        }

        /// <summary>
        /// Setting the list of accounts in the class
        /// </summary>
        /// <param name="accounts">List of accounts the list should be set to</param>
        /// <returns>no return</returns>
        public async Task SetListOfAccounts(List<Account> accounts)
        {
            this.Accounts = accounts;
        }

        /// <summary>
        /// Requesting an account by its username
        /// </summary>
        /// <param name="username">Account's username</param>
        /// <returns>Account that has the corresponding username</returns>
        public async Task<Account> RequestAccount(string username)
        {
            return await model.RequestAccount(username);
        }

        /// <summary>
        /// Requesting a user by its ID
        /// </summary>
        /// <param name="userID">User's ID that is requested</param>
        /// <returns>Account that has the given ID</returns>
        public async Task<Account> RequestAccountById(string userID)
        {
            return await model.RequestAccountByID(userID);
        }

        /// <summary>
        /// Adding a friend
        /// </summary>
        /// <param name="users">List containing to users who should become each others friends</param>
        /// <returns><no return/returns>
        public async Task AddFriend(List<User> users)
        {
            await model.AddFriend(users);
        }

        public async Task EditAccount(Account account)
        {
            await model.EditAccount(account);
        }

        /// <summary>
        /// Removing a topic from a given user
        /// </summary>
        /// <param name="userId">User from whom the topic should be removed</param>
        /// <param name="topic">Topic to be removed</param>
        /// <returns>no return</returns>
        public async Task RemoveTopicFromUser(string userId, string topic)
        {
            Console.Out.WriteLine(userId);
            Console.Out.WriteLine(topic);
            model.RemoveTopicFromUser(userId, topic);
        }

        /// <summary>
        /// Adding a topic to a User
        /// </summary>
        /// <param name="userId">Who the topic should be added to</param>
        /// <param name="topic">Topic to be added</param>
        /// <returns>no return</returns>
        public async Task AddTopicToUser(string userId, string topic)
        {
            model.AddTopicToUser(userId, topic);
        }

        /// <summary>
        /// Removing a friend
        /// </summary>
        /// <param name="userId">Friend 1</param>
        /// <param name="friendId">Friend 2</param>
        /// <returns>no return</returns>
        public async Task RemoveFriend(string userId, string friendId)
        {
            await model.RemoveFriend(userId, friendId);
            await model.DeletePrivateChatroom(userId, friendId);
        }
    }
}