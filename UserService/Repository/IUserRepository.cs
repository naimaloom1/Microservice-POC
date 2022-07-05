using UserService.Database;

namespace UserService.Repository
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsersList();
        IQueryable<User> GetUsers();
        User GetUser(int id);
        bool CreateUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(User user);
        bool Save();
        bool UserExists(string mobileNo);
        bool UserExists(int Id);

    }
}
