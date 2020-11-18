using System.Threading.Tasks;
using UserService.Exceptions;
using UserService.Models;
using UserService.Repository;
namespace UserService.Services
{
    //Inherit the respective interface and implement the methods in 
    // this class i.e UserService by inheriting IUserService
    public class UserService : IUserService
    {
        /*
         * UserRepository should  be injected through constructor injection. 
         * Please note that we should not create USerRepository object using the new keyword
         */
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<bool> AddUser(UserProfile user)
        {
            var currentUser = _userRepository.GetUser(user.UserId).Result;
            if (currentUser != null)
            {
                throw new UserAlreadyExistsException($"{user.UserId} is already in use");
            }
            return _userRepository.AddUser(user);
        }

        public Task<bool> DeleteUser(string userId)
        {
            var currentUser = _userRepository.GetUser(userId).Result;
            if (currentUser == null)
            {
                throw new UserNotFoundException($"This user id doesn't exist");
            }
            return _userRepository.DeleteUser(userId);
        }

        public Task<UserProfile> GetUser(string userId)
        {
            var result= _userRepository.GetUser(userId);
            if(result.Result==null)
            {
                throw new UserNotFoundException($"This user id doesn't exist");
            }
            return result;
        }

        public Task<bool> UpdateUser(string userId, UserProfile user)
        {
            var result = _userRepository.UpdateUser(user);
            if (!result.Result)
            {
                throw new UserNotFoundException($"This user id doesn't exist");                
            }
            return result;            
        }
    }
}
