using DataModels.Authentication;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IPasswordHasherService
    {
        Task<HashedPassword> HashPassword(string password, byte[] salt = null);
    }
}