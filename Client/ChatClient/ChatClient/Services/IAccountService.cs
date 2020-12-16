using System.Threading.Tasks;
using Models;

namespace Services
{
    public interface IAccountService
    {
        Task<Account> Login(string username, string password);
        Task Register(Account account);
        Task AddFriend(string UserID, Account account);
        public Task DeleteProfile(string userID);
        public Task AddTopicToProfile(string topic, string userId);
        public Task RemoveTopicFromProfile(string topic, string userId);
        public Task EditProfile(Account account);
        public Task<Account> GetUser(string username);
        public Task RemoveFriend(string userId, string friendId);
    }
}