using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Application.Services
{
    public interface IAccountService
    {
        public Task Register(Account account);
        public Task<Account> LogIn(string username, string password);
        public Task RemoveAccount(string accountID);
        public Task<IList<Account>> GetAllAccounts();
        public Task RequestAccounts();
        public Task SetListOfAccounts(List<Account> accounts);
        public Task<Account> RequestAccount(string username);

        public Task<Account> RequestAccountById(string userID);
        public Task AddFriend(List<User> users);
        public Task EditAccount(Account account);
        public Task removeTopicFromUser(string userId, string topic);
        public Task addTopicToUser(string userId, string topic);
    }
}