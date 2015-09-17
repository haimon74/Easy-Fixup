using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class FaqCategoryMap : EntityTypeConfiguration<FaqCategory>
    {
        public FaqCategoryMap()
        {
            // Primary Key
            this.HasKey(t => t.fc_id);

            // Properties
            this.Property(t => t.fc_name)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("FaqCategories");
            this.Property(t => t.fc_id).HasColumnName("fc_id");
            this.Property(t => t.fc_name).HasColumnName("fc_name");
            this.Property(t => t.fc_active).HasColumnName("fc_active");
        }
    }
}
