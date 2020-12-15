using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Models;
using Application.SCMediator;

namespace Application.Services
{
    public class AccountsServiceImpl : IAccountService
    {
        public List<Account> Accounts { get; set; }
        public Model model;

        public AccountsServiceImpl(Model modelManager)
        {
            this.model = modelManager;
            Accounts = new List<Account>();
        }

        public async Task Register(Account account)
        {
            this.Accounts.Add(account);
            await model.Register(account);
        }

        public Task Register(string account)
        {
            throw new NotImplementedException();
        }

        public async Task<Account> LogIn(string username, string password)
        {
            Account account = null;
            foreach (var VARIABLE in Accounts)
            {
                if (VARIABLE.Pass.Equals(password) & VARIABLE.Username.Equals(username))
                {
                    account = VARIABLE;
                }
            }

            return account;
        }

        public async Task RemoveAccount(string accountID)
        {
            await model.RemoveUser(accountID);
        }

        public async Task RequestAccounts()
        {
            Accounts = await model.RequestUsers();

        }

        public async Task<IList<Account>> GetAllAccounts()
        {
            await RequestAccounts();
            return Accounts;
        }

        public async Task SetListOfAccounts(List<Account> accounts)
        {
            this.Accounts = accounts;
        }

        public async Task<Account> RequestAccount(string username)
        {
            return await model.RequestAccount(username);
        }

        public async Task<Account> RequestAccountById(string userID)
        {
            return await model.RequestAccountByID(userID);
        }

        public async Task AddFriend(List<User> users)
        {
            await model.AddFriend(users);
        }

        public async Task EditAccount(Account account)
        {
            await model.EditAccount(account);
        }
        public async Task RemoveTopicFromUser(string userId, string topic)
        {
            Console.Out.WriteLine(userId);
            Console.Out.WriteLine(topic);
            model.RemoveTopicFromUser(userId,topic);
        }

        public async Task AddTopicToUser(string userId, string topic)
        {
            model.AddTopicToUser(userId,topic);
        }

        public async Task RemoveFriend(string userId, string friendId)
        {
            await model.RemoveFriend(userId, friendId);
            await model.DeletePrivateChatroom(userId, friendId);
        }
    }
}