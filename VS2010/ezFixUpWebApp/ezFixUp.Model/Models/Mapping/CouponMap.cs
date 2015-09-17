using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class CouponMap : EntityTypeConfiguration<Coupon>
    {
        public CouponMap()
        {
            // Primary Key
            this.HasKey(t => t.c_id);

            // Properties
            this.Property(t => t.c_code)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.c_username)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("Coupons");
            this.Property(t => t.c_id).HasColumnName("c_id");
            this.Property(t => t.c_code).HasColumnName("c_code");
            this.Property(t => t.c_username).HasColumnName("c_username");
            this.Property(t => t.c_start_date).HasColumnName("c_start_date");
            this.Property(t => t.c_due_date).HasColumnName("c_due_date");
            this.Property(t => t.c_max_use).HasColumnName("c_max_use");
            this.Property(t => t.c_type_id).HasColumnName("c_type_id");
            this.Property(t => t.c_used).HasColumnName("c_used");
            this.Property(t => t.c_only_new_user).HasColumnName("c_only_new_user");
        }
    }
}
