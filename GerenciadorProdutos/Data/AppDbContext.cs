using Microsoft.EntityFrameworkCore;
using GerenciadorProdutos.Models.Category;
using GerenciadorProdutos.Models.Products;
using GerenciadorProdutos.Models.Users;

namespace GerenciadorProdutos.Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração de Product
            modelBuilder.ApplyConfiguration(new ProductConfiguration());

            // Configuração de Category
            modelBuilder.Entity<Category>()
                .HasKey(c => c.CategoryId);

            modelBuilder.Entity<Category>()
                .Property(c => c.Categoria)
                .IsRequired()
                .HasMaxLength(50);

            // Relacionamento entre Product e Category (1:N)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category) // Um Product tem uma Category
                .WithMany(c => c.Products) // Uma Category pode ter muitos Products
                .HasForeignKey(p => p.CategoryId); // Chave estrangeira

            // Configuração de User
            modelBuilder.Entity<User>().HasKey(u => u.Id);
        }
    }
}
