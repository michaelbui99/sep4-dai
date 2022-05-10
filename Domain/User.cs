using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Password { get; set; }
    }
}