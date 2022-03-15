using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace PwdApp.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public string Email { get; set; }
    }
}