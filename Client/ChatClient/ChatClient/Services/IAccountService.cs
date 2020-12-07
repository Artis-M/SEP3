using System.Threading.Tasks;
using Application.Models;
using Models;

namespace Services
{
    public interface IAccountService
    {
        Task<Account> Login(string username, string password);
        Task Register(string username, string password);
    }
}