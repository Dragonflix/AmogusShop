using Microsoft.EntityFrameworkCore;

namespace AmogusShop.Models
{
    public class User
    {
        public User(string name,string email)
        {
            Name = name;
            Email = email;
            Balance = 0;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } 
        public float Balance { get; set; }
    }
}
