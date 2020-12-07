using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Models;
using Application.SCMediator;

namespace Application.Services
{
    public class AccountsServiceImpl : IAccountService
    {
        public List<Account> Accounts { get; set; }
        public Model model;
        private bool wait=false;
        private AutoResetEvent waiter = new AutoResetEvent(false);

        public AccountsServiceImpl(Model modelManager)
        {
            this.model = modelManager;
            
        }

        public async Task Register(Account account)
        {
            this.Accounts.Add(account);
            await model.Register(account);
        }

        public Task Register(string account)
        {
            throw new NotImplementedException();
        }

        public async Task<Account> LogIn(string username, string password)
        {
            Account account = null;
            foreach (var VARIABLE in Accounts)
            {
                if (VARIABLE.Pass.Equals(password) & VARIABLE.Username.Equals(username))
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
                if (VARIABLE._id.Equals(accountID))
                {
                    Accounts.Remove(VARIABLE);
                }
            }

            await model.RemoveUser(accountID);
        }
        
        public async Task RequestAccounts()
        {
             await model.RequestUsers();
        }

        public async Task<IList<Account>> GetAllAccounts()
        {
            Console.WriteLine("Before await RequestAccounts");
            await RequestAccounts();
            Console.WriteLine("Awaiting Write Line");
            waiter.WaitOne();
            Console.WriteLine("WERE RETURNING");
            return Accounts;
        }
        public async Task SetListOfAccounts(List<Account> accounts)
        { 
            this.Accounts = accounts;
            Console.WriteLine("Before waiter reset");
            waiter.Set();
            Console.WriteLine("After waiter reset");
        }
    }
}