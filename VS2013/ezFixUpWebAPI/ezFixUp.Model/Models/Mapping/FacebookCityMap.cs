using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class FacebookCityMap : EntityTypeConfiguration<FacebookCity>
    {
        public FacebookCityMap()
        {
            // Primary Key
            this.HasKey(t => t.fc_key);

            // Properties
            this.Property(t => t.fc_key)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.fc_name)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.fc_subtext)
                .IsRequired()
                .HasMaxLength(30);

            // Table & Column Mappings
            this.ToTable("FacebookCities");
            this.Property(t => t.fc_key).HasColumnName("fc_key");
            this.Property(t => t.fc_name).HasColumnName("fc_name");
            this.Property(t => t.fc_subtext).HasColumnName("fc_subtext");
        }
    }
}
