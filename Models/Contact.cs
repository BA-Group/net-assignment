using System.ComponentModel.DataAnnotations;

namespace net_assignment.Models
{
    public class Contact
    {
        public long Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
    }
}