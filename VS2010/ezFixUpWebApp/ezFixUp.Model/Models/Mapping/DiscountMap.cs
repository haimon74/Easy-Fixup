using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class DiscountMap : EntityTypeConfiguration<Discount>
    {
        public DiscountMap()
        {
            // Primary Key
            this.HasKey(t => t.d_id);

            // Properties
            this.Property(t => t.d_match)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Discounts");
            this.Property(t => t.d_id).HasColumnName("d_id");
            this.Property(t => t.d_amount).HasColumnName("d_amount");
            this.Property(t => t.d_type).HasColumnName("d_type");
            this.Property(t => t.d_field).HasColumnName("d_field");
            this.Property(t => t.d_match).HasColumnName("d_match");
        }
    }
}
