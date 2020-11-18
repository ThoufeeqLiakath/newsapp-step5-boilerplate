using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.Repository
{
    /*
	 * Should not modify this interface. You have to implement these methods of interface 
     * in corresponding Implementation classes
	 */
    public interface IUserRepository
    {
        Task<bool> AddUser(UserProfile user);
        Task<UserProfile> GetUser(string userId);
        Task<bool> UpdateUser(UserProfile user);
        Task<bool> DeleteUser(string userId);
    }
}
