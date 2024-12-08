using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Nome)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(p => p.Descricao)
               .HasMaxLength(255);

        builder.Property(p => p.Status)
               .HasMaxLength(20);

        builder.Property(p => p.Preco)
               .HasColumnType("decimal(10,2)");

        builder.Property(p => p.QuantidadeEstoque)
               .HasDefaultValue(0);

        builder.HasOne(p => p.Category)
               .WithMany(c => c.Products)
               .HasForeignKey(p => p.CategoryId)
               .OnDelete(DeleteBehavior.Cascade); // Exclusão em cascata
    }
}

