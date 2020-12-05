using System.Threading.Tasks;

namespace Services
{
    public interface LoginService
    {
        Task<string> Login(string username, string password);
        Task Register(string username, string password);
    }
}