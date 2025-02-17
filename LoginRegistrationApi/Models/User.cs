using System.ComponentModel.DataAnnotations;

namespace LoginRegistrationApi.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Surname { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        // Parameterless constructor for EF Core
        public User() { }

        // Custom constructor for manual object creation if needed
        public User(string name, string surname, string phone, string email, string password)
        {
            Name = name;
            Surname = surname;
            Phone = phone;
            Email = email;
            Password = password;
        }
    }
}
