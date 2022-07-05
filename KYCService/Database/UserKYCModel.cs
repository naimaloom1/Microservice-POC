namespace KYCService.Database
{
    public class UserKYCModel
    {
        public int UserKYCId { get; set; }
        public int UserId { get; set; }
        public string? KYCStatus { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public string? MobileNo { get; set; }
        public string? Title { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Sex { get; set; }
        public string? Role { get; set; }
        public string? PasswordHash { get; set; }
        public bool Status { get; set; }
    }
}
