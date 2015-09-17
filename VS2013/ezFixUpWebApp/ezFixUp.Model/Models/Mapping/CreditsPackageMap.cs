using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class CreditsPackageMap : EntityTypeConfiguration<CreditsPackage>
    {
        public CreditsPackageMap()
        {
            // Primary Key
            this.HasKey(t => t.cp_id);

            // Properties
            this.Property(t => t.cp_name)
                .IsRequired()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("CreditsPackages");
            this.Property(t => t.cp_id).HasColumnName("cp_id");
            this.Property(t => t.cp_name).HasColumnName("cp_name");
            this.Property(t => t.cp_quantity).HasColumnName("cp_quantity");
            this.Property(t => t.cp_price).HasColumnName("cp_price");
        }
    }
}
