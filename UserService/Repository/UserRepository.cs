using Microsoft.EntityFrameworkCore;
using UserService.Database;

namespace UserService.Repository
{
    public class UserRepository : IUserRepository
    {

        private readonly DatabaseContext _db;

        public UserRepository(DatabaseContext db)
        {
            _db = db;
        }
        public IQueryable<User> GetUsers()
        {
            return _db.Users.AsQueryable();

        }
        bool IUserRepository.CreateUser(User user)
        {
            try
            {
                _db.Users.Add(user);
                return Save();
            }
            catch(Exception ex) { 
            return false;
            }
        }

        bool IUserRepository.DeleteUser(User user)
        {
            _db.Users.Remove(user);
            return Save();
        }

        public User GetUser(int id)
        {
            return _db.Users.FirstOrDefault(x => x.UserId == id);
        }


        public IEnumerable<User> GetUsersList()
        {
            return _db.Users.ToList();
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        bool IUserRepository.UpdateUser(User user)
        {
            _db.Users.Update(user);
            return Save();
        }
        public bool UserExists(string mobileNo)
        {
            bool value = _db.Users.Any(y => y.MobileNo.ToLower().Trim() == mobileNo.ToLower().Trim());
            return value;
        }

        public bool UserExists(int Id)
        {
            return _db.Users.Any(x => x.UserId == Id);
        }

    }
}
