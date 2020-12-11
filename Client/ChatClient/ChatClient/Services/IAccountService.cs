using System.Threading.Tasks;
using Application.Models;
using Models;

namespace Services
{
    public interface IAccountService
    {
        Task<Account> Login(string username, string password);
        Task Register(Account account);

        Task AddFriend(string UserID, Account account);
        public Task DeleteProfile(string userID);
        public Task addTopicToProfile(string topic, string userId);
        public Task removeTopicFromProfile(string topic, string userId);
        public Task editProfile(Account account);
    }
}