using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class User
    {
        public int UserId { get; set; }
        [Required]
        [Display(Name = "Mobile No")]
        [Phone(ErrorMessage = "Phone is not valid.")]      
        public string? MobileNo { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required]
        public string? Sex { get; set; }
        [Required]
        public string? Role { get; set; }
        [Required]
        [Display(Name = "Password")]
        public string? PasswordHash { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
