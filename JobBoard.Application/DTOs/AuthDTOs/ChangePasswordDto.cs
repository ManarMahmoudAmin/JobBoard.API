using System.ComponentModel.DataAnnotations;

namespace JobBoard.Application.DTOs.AuthDTOs

{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")] 
        public string Email { get; set; }

        [Required(ErrorMessage = "Old password is required")]
        [StringLength(100, ErrorMessage = "Old password must be at least 6 characters long", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "New password is required")]
        [StringLength(100, ErrorMessage = "New password must be at least 6 characters long", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm new password is required")]
        [StringLength(100, ErrorMessage = "Confirm new password must be at least 6 characters long", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Confirm new password does not match new password")]
        public string ConfirmNewPassword { get; set; }
    }
}
