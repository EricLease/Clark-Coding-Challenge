using System.ComponentModel.DataAnnotations;

namespace ClarkCodingChallenge.Models
{
    public class ContactViewModel
    {
        [Required]
        public int Id { get; set; } = -1;

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
    }
}
