using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Model;

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
    }
}