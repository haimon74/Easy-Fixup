using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class ChatTranslationMap : EntityTypeConfiguration<ChatTranslation>
    {
        public ChatTranslationMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ct_key, t.l_id });

            // Properties
            this.Property(t => t.ct_key)
                .IsRequired()
                .HasMaxLength(400);

            this.Property(t => t.l_id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ct_value)
                .IsRequired()
                .HasMaxLength(400);

            // Table & Column Mappings
            this.ToTable("ChatTranslations");
            this.Property(t => t.ct_key).HasColumnName("ct_key");
            this.Property(t => t.l_id).HasColumnName("l_id");
            this.Property(t => t.ct_value).HasColumnName("ct_value");
            this.Property(t => t.ct_clientside).HasColumnName("ct_clientside");
        }
    }
}
