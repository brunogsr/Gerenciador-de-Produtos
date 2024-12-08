using GerenciadorProdutos.Models.Category;
using GerenciadorProdutos.Models.Users;
using Microsoft.EntityFrameworkCore;

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

            // Configuração de User
            modelBuilder.Entity<User>().HasKey(u => u.Id);
        }
    }
}
