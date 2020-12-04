using System;
using System.Collections.Generic;
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
            this.Accounts = new List<Account>();
        }

        public async Task Register(Account account)
        {
            this.Accounts.Add(account);
            await model.Register(account);
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

        public async Task RemoveAccount(string accountID)
        {
            foreach (var VARIABLE in Accounts)
            {
                if (VARIABLE.Id.Equals(accountID))
                {
                    Accounts.Remove(VARIABLE);
                }
            }

            await model.RemoveUser(accountID);
        }

        public async Task<IList<Account>> GetAllAccounts()
        {
            await RequestAccounts();
            Console.Out.WriteLine(Accounts.Count);
            return Accounts;
        }
        public async Task<List<Account>> RequestAccounts()
        {
            return await model.RequestUsers();
        }

        public async Task SetListOfAccounts(List<Account> accounts)
        {
            this.Accounts = accounts;

        }
    }
}