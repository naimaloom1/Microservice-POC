using Microsoft.EntityFrameworkCore;
using KYCService.Database;

namespace KYCService.Repository
{
    public class UserKYCRepository : IUserKYC
    {
        private readonly DatabaseContext _db;
        public UserKYCRepository(DatabaseContext db)
        {
            _db = db;
        }

        public IQueryable<User> GetPendingKYC()
        {
            IQueryable<User> users = (from u in _db.Users join r in _db.UserKYC on u.UserId equals r.UserId select u );
            var subselect = (from b in _db.UserKYC select b.UserId).ToList();

            var result = from c in _db.Users where !subselect.Contains(c.UserId) select c;
            var ApprovedIds = _db.UserKYC.Select(x => x.UserId).ToArray();

            IQueryable<User> user = _db.Users.Where(p => !ApprovedIds.Contains(p.UserId)).AsQueryable();
            return user;
        }
        public IQueryable<UserKYCModel> GetApprovalPending()
        {
            
                var data = (from u in _db.Users join r in _db.UserKYC on u.UserId equals r.UserId
                                      where r.KYCStatus!= "Approved"
                            select new UserKYCModel(){ 
                                          UserId = u.UserId,
                                          Title = u.Title,
                                          FirstName = u.FirstName,
                                          LastName = u.LastName,   
                                          MobileNo = u.MobileNo,    
                                          Email = u.Email,
                                          KYCStatus = r.KYCStatus,
                                         UserKYCId = r.UserKYCId

                                      });
            //var subselect = (from b in _db.UserKYC select b.UserId).ToList();

            //var result = from c in _db.Users where !subselect.Contains(c.UserId) select c;
            //var ApprovedIds = _db.UserKYC.Select(x => x.UserId).ToArray();

            //IQueryable<User> user = _db.Users.Where(p => !ApprovedIds.Contains(p.UserId)).AsQueryable();
            IQueryable<UserKYCModel> users = data.AsQueryable();
            return users;
        }

        public IQueryable<UserKYCModel> GetKYCStatus()
        {
            //IEnumerable<User> person = _db.Users.Include(p => p.Users).ToList();
            //(from p in _db.Users // get person table as p
            //      join e in _db.UserKYC // implement join as e in EmailAddresses table
            //      on p.UserId equals e.UserId //implement join on rows where p.BusinessEntityID == e.BusinessEntityID
            //      select new { p, e }).ToList();
            var data = (from u in _db.Users
                        join r in _db.UserKYC on u.UserId equals r.UserId
                      
                        select new UserKYCModel()
                        {
                            UserId = u.UserId,
                            Title = u.Title,
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            MobileNo = u.MobileNo,
                            Email = u.Email,
                            KYCStatus = r.KYCStatus,
                            UserKYCId = r.UserKYCId

                        });
            IQueryable<UserKYCModel> users = data.AsQueryable();
            return users;
        }

        public UserKYCModel GetKYCUser(int id)
        {
            UserKYCModel usersdata = new UserKYCModel();
            var usercompany = (from u in _db.Users
                              join c in _db.UserKYC
                              on u.UserId equals c.UserId into cu
                              from co in cu.DefaultIfEmpty()
                               where u.UserId == id
                               select new
                              {
                                  u,
                                  co,
                              }).ToList();
          //  var result = usercompany.ToList()
            foreach (var data in usercompany)
            {
                usersdata.UserId = data.u.UserId;
                usersdata.Title= data.u.Title;
                usersdata.FirstName= data.u.FirstName;
                usersdata.LastName= data.u.LastName;
                usersdata.Email= data.u.Email;
                usersdata.MobileNo= data.u.MobileNo;
                usersdata.Sex= data.u.Sex;
                if (data.co != null)
                {
                    usersdata.KYCStatus = data.co.KYCStatus;
                    usersdata.UserKYCId = data.co.UserKYCId;
                }
            }
            return usersdata;
        }
        public bool UpdateKYC(UserKYC userKYC)
        {
            _db.UserKYC.Update(userKYC);
            return Save();
        }
        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }

        bool IUserKYC.CreateUserKYC(UserKYC userKYC)
        {
            _db.UserKYC.Add(userKYC);
            return Save();
        }
    }
}
