using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Models;

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
    }
}