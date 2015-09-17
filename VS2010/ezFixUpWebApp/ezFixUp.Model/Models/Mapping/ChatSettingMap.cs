using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class ChatSettingMap : EntityTypeConfiguration<ChatSetting>
    {
        public ChatSettingMap()
        {
            // Primary Key
            this.HasKey(t => t.cs_key);

            // Properties
            this.Property(t => t.cs_key)
                .IsRequired()
                .HasMaxLength(80);

            this.Property(t => t.cs_value)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("ChatSettings");
            this.Property(t => t.cs_key).HasColumnName("cs_key");
            this.Property(t => t.cs_value).HasColumnName("cs_value");
        }
    }
}
