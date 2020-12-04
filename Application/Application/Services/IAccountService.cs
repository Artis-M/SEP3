using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Model;

namespace Application.Services
{
    public interface IAccountService
    {
        public Task Register(string account);
        public Task<Account> LogIn(string username, string password);
        public Task RemoveAccount(Account account);
        public Task<IList<Account>> GetAllAccounts();
    }
}