
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CartService.Models;
namespace CartService.Configuration
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasOne(c => c.User)
                  .WithOne(u => u.Cart)
                  .HasForeignKey<Cart>(c => c.UserId);

            builder.HasMany(c => c.Items)
                .WithOne(p => p.Cart)
                .HasForeignKey(p => p.CartId)
                .IsRequired(false); // Робимо зв'язок необов'язковим;
        }
    }
}
