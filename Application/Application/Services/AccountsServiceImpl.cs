using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Model;
using Application.SCMediator;

namespace Application.Services
{
    public class AccountsServiceImpl : IAccountService
    {
        public List<Account> Accounts { get; set; }
        private ServiceImpl service;

        public AccountsServiceImpl()
        {
            this.Accounts = new List<Account>();
            service = new ServiceImpl();
        }

        public async Task Register(string username)
        {
           await service.requestUser(username);
            //this.Accounts.Add(account);
        }

        public async Task<Account> LogIn(string username, string password)
        {
            Account account = null;
            foreach (var VARIABLE in Accounts)
            {
                if (VARIABLE.password.Equals(password) & VARIABLE.Username.Equals(username))
                {
                    account = VARIABLE;
                }
            }

            return account;
        }

        public async Task RemoveAccount(Account account)
        {
            foreach (var VARIABLE in Accounts)
            {
                if (VARIABLE.Id.Equals(account.Id))
                {
                    Accounts.Remove(VARIABLE);
                }
            }
        }

        public async Task<IList<Account>> GetAllAccounts()
        {
            return Accounts;
        }
    }
}