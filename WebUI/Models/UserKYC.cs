namespace WebUI.Models
{
    public class UserKYC
    {
        public int UserKYCId { get; set; }
        public int UserId { get; set; }
        public string? KYCStatus { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
