using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Model;
using Application.SCMediator;

namespace Application.Services
{
    public class AccountsServiceImpl : IAccountService
    {
        public List<Account> Accounts { get; set; }
        public Tier2.Model.Model model;
        private ChatServiceImp service;

        public AccountsServiceImpl()
        {
            service = new ChatServiceImp();
            this.Accounts = new List<Account>();
        }

        public async Task Register(string account)
        {
            //this.Accounts.Add(account);
            await service.requestUser(account);
            //await model.Register(account);
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