using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class FacebookCountryMap : EntityTypeConfiguration<FacebookCountry>
    {
        public FacebookCountryMap()
        {
            // Primary Key
            this.HasKey(t => new { t.fc_id, t.fc_country_code, t.fc_name });

            // Properties
            this.Property(t => t.fc_id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.fc_country_code)
                .IsRequired()
                .HasMaxLength(3);

            this.Property(t => t.fc_name)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("FacebookCountries");
            this.Property(t => t.fc_id).HasColumnName("fc_id");
            this.Property(t => t.fc_country_code).HasColumnName("fc_country_code");
            this.Property(t => t.fc_name).HasColumnName("fc_name");
            this.Property(t => t.fc_supports_region).HasColumnName("fc_supports_region");
        }
    }
}
