using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class ECardCategoryMap : EntityTypeConfiguration<ECardCategory>
    {
        public ECardCategoryMap()
        {
            // Primary Key
            this.HasKey(t => t.ecc_id);

            // Properties
            this.Property(t => t.ecc_name)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ECardCategories");
            this.Property(t => t.ecc_id).HasColumnName("ecc_id");
            this.Property(t => t.ecc_name).HasColumnName("ecc_name");
        }
    }
}
