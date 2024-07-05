using BankingApp.Core.Application.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BankingApp.Core.Application.ViewModels.User
{
    public class SaveUserViewModel
    {
        public string Id { get; set; }

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

        public bool EmailConfirmed { get; set; }

        [Required]
        [StringLength(20)]
        public string IdCard { get; set; }

        public byte Status { get; set; }

        public List<RoleTypes> Roles { get; set; }

        public string? PhotoUrl { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? FIle { get; set; }
    }
}
