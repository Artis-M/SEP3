using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Models;

namespace Application.Services
{
    public interface IAccountService
    {
        public Task Register(Account account);
        public Task RemoveAccount(string accountID);
        public Task<IList<Account>> GetAllAccounts();
        public Task SetListOfAccounts(List<Account> accounts);
        public Task<Account> RequestAccount(string username);
        public Task<Account> RequestAccountById(string userID);
        public Task AddFriend(List<User> users);
        public Task EditAccount(Account account);
        public Task RemoveTopicFromUser(string userId, string topic);
        public Task AddTopicToUser(string userId, string topic);
        public Task RemoveFriend(string userId, string friendId);
    }
}