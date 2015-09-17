using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class CouponTypeMap : EntityTypeConfiguration<CouponType>
    {
        public CouponTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.ct_id);

            // Properties
            this.Property(t => t.ct_name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ct_description)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("CouponTypes");
            this.Property(t => t.ct_id).HasColumnName("ct_id");
            this.Property(t => t.ct_name).HasColumnName("ct_name");
            this.Property(t => t.ct_description).HasColumnName("ct_description");
        }
    }
}
