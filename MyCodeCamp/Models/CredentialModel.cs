using System;
using System.ComponentModel.DataAnnotations;

namespace MyCodeCamp.Models
{
    public class CredentialModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
