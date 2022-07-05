using KYCService.Database;

namespace KYCService.Repository
{
    public interface IUserKYC
    {
        IQueryable<User> GetPendingKYC();
        IQueryable<UserKYCModel> GetApprovalPending();
        IQueryable<UserKYCModel> GetKYCStatus();
        UserKYCModel GetKYCUser(int id);
        bool UpdateKYC(UserKYC userKYC);
        bool CreateUserKYC(UserKYC userKYC);
    }
}
