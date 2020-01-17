using System;
using System.ComponentModel.DataAnnotations;

namespace Flora_Queen_Project.Models
{
    public class AccountManageViewModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime? Birthday { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
        public ApplicationUser.UserStatusEnum? UserStatus { get; set; }
    }
    public class AccountManageRoleViewModel
    {
        [Required]
        public string Id { get; set; }
        public string UserName { get; set; }
        [Required]
        public RoleEnum Role { get; set; }
        public enum RoleEnum
        {
            [Display(Name = "Admin")]
            Admin = 1,
            [Display(Name = "Moderator")]
            Mod = 2,
            [Display(Name = "User")]
            User = 3
        }

        
    }
}