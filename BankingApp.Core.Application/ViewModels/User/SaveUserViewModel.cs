using BankingApp.Core.Application.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BankingApp.Core.Application.ViewModels.User
{
    public class SaveUserViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Debe colocar un nombre")]
        [StringLength(50, ErrorMessage = "El nombre no puede superar los 50 caracteres")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Debe colocar un apellido")]
        [StringLength(50, ErrorMessage = "El apellido no puede superar los 50 caracteres")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Debe colocar un nombre de usuario")]
        [StringLength(30, ErrorMessage= "El nombre de usuario no puede superar los 30 caracteres")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Debe colocar un correo")]
        [EmailAddress(ErrorMessage = "Dirección de correo inválida")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Debe colocar una cédula")]
        [StringLength(13)]
        [RegularExpression(@"^\d{3}-\d{7}-\d$", ErrorMessage = "La cédula debe tener el formato 000-0000000-0")]

        public string IdCard { get; set; }

        public byte Status { get; set; }

        [Range(0, 1, ErrorMessage = "Debe seleccionar un tipo de usuario")]
        public RoleTypes? Role { get; set; }

        public string? PhotoUrl { get; set; }

        [StringLength(100, MinimumLength = 6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? File { get; set; }

        public double? InitialAmount { get; set; }
        public double? AditionalAmount { get; set; }
    }
}
