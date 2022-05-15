using Microsoft.EntityFrameworkCore;

namespace AmogusShop.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<AMOGUS> Amogs { get; set; } = null!;
        public DbSet<CartItem> CartItems { get; set; } = null!;
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}

