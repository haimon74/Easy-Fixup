using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class GiftCategoryMap : EntityTypeConfiguration<GiftCategory>
    {
        public GiftCategoryMap()
        {
            // Primary Key
            this.HasKey(t => t.gc_id);

            // Properties
            this.Property(t => t.gc_name)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("GiftCategories");
            this.Property(t => t.gc_id).HasColumnName("gc_id");
            this.Property(t => t.gc_name).HasColumnName("gc_name");
            this.Property(t => t.gc_active).HasColumnName("gc_active");
        }
    }
}
