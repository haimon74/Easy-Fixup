using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class AdsCategoryMap : EntityTypeConfiguration<AdsCategory>
    {
        public AdsCategoryMap()
        {
            // Primary Key
            this.HasKey(t => t.ac_id);

            // Properties
            this.Property(t => t.ac_title)
                .IsRequired()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("AdsCategories");
            this.Property(t => t.ac_id).HasColumnName("ac_id");
            this.Property(t => t.ac_parentid).HasColumnName("ac_parentid");
            this.Property(t => t.ac_title).HasColumnName("ac_title");

            // Relationships
            this.HasOptional(t => t.AdsCategory1)
                .WithMany(t => t.AdsCategories1)
                .HasForeignKey(d => d.ac_parentid).WillCascadeOnDelete(false);

        }
    }
}
