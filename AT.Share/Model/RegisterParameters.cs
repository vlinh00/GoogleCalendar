
using System.ComponentModel.DataAnnotations;


namespace AT.Share.Model
{
    public class RegisterParameters
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public string? FullName { get; set; }

        public int DepartmentId { get; set; }

        public int GroupUserId { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        public string PasswordConfirm { get; set; }
    }
}
