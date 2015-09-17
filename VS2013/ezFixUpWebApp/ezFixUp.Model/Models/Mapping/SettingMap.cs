using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class SettingMap : EntityTypeConfiguration<Setting>
    {
        public SettingMap()
        {
            // Primary Key
            this.HasKey(t => t.s_key);

            // Properties
            this.Property(t => t.s_key)
                .IsRequired()
                .HasMaxLength(80);

            this.Property(t => t.s_value)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Settings");
            this.Property(t => t.s_key).HasColumnName("s_key");
            this.Property(t => t.s_value).HasColumnName("s_value");
        }
    }
}
