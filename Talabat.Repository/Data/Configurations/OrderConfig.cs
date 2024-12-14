using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data.Configurations
{
    internal class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(O => O.Status)
                .HasConversion(OStatus => OStatus.ToString(), OStatus => (OrderStatus) Enum.Parse(typeof(OrderStatus), OStatus));

            builder.Property(O => O.SubTotal)
                .HasColumnType("decimal(18,2)");

            builder.OwnsOne(O => O.ShippingAddress, X => X.WithOwner());

            builder.HasOne(O => O.DeliveryMethod)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction);
        
        }
    }
}
