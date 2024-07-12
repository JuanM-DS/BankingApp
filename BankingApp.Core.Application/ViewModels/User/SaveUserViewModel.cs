using BankingApp.Core.Application.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BankingApp.Core.Application.ViewModels.User
{
    public class SaveUserViewModel
    {
        public string? Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(20)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        public string IdCard { get; set; }

        public byte Status { get; set; }

        public RoleTypes? Role { get; set; }

        public string? PhotoUrl { get; set; }

        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? File { get; set; }

        public double? InitialAmount { get; set; }
        public double? AditionalAmount { get; set; }
    }
}
