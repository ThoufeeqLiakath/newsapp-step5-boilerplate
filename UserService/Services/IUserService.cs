using System.Threading.Tasks;
using UserService.Models;
namespace UserService.Services
{
    /*
	 * Should not modify this interface. You have to implement these methods of interface 
     * in corresponding Implementation classes
	 */
    public interface IUserService
    {
        Task<bool> AddUser(UserProfile user);
        Task<UserProfile> GetUser(string userId);
        Task<bool> UpdateUser(string userId,UserProfile user);
        Task<bool> DeleteUser(string userId);
    }
}
