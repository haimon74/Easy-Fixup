using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class CouponUsMap : EntityTypeConfiguration<CouponUs>
    {
        public CouponUsMap()
        {
            // Primary Key
            this.HasKey(t => new { t.cu_code, t.cu_username });

            // Properties
            this.Property(t => t.cu_code)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.cu_username)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("CouponUses");
            this.Property(t => t.cu_code).HasColumnName("cu_code");
            this.Property(t => t.cu_username).HasColumnName("cu_username");
        }
    }
}
