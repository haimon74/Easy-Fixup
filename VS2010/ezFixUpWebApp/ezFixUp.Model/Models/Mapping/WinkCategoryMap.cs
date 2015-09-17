using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class WinkCategoryMap : EntityTypeConfiguration<WinkCategory>
    {
        public WinkCategoryMap()
        {
            // Primary Key
            this.HasKey(t => t.wc_id);

            // Properties
            this.Property(t => t.wc_name)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("WinkCategories");
            this.Property(t => t.wc_id).HasColumnName("wc_id");
            this.Property(t => t.wc_name).HasColumnName("wc_name");
            this.Property(t => t.wc_language_id).HasColumnName("wc_language_id");
            this.Property(t => t.wc_active).HasColumnName("wc_active");

            // Relationships
            this.HasRequired(t => t.Language)
                .WithMany(t => t.WinkCategories)
                .HasForeignKey(d => d.wc_language_id).WillCascadeOnDelete(false);

        }
    }
}
