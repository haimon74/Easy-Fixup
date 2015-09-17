using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class ChatMessageMap : EntityTypeConfiguration<ChatMessage>
    {
        public ChatMessageMap()
        {
            // Primary Key
            this.HasKey(t => t.cm_id);

            // Properties
            this.Property(t => t.cm_senderdisplayname)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.cm_text)
                .IsRequired()
                .HasMaxLength(2000);

            this.Property(t => t.cm_texthtml)
                .IsRequired()
                .HasMaxLength(2000);

            // Table & Column Mappings
            this.ToTable("ChatMessages");
            this.Property(t => t.cm_id).HasColumnName("cm_id");
            this.Property(t => t.cm_type).HasColumnName("cm_type");
            this.Property(t => t.cm_time).HasColumnName("cm_time");
            this.Property(t => t.cr_id).HasColumnName("cr_id");
            this.Property(t => t.cu_id_sender).HasColumnName("cu_id_sender");
            this.Property(t => t.cm_senderdisplayname).HasColumnName("cm_senderdisplayname");
            this.Property(t => t.cu_id_target).HasColumnName("cu_id_target");
            this.Property(t => t.cm_text).HasColumnName("cm_text");
            this.Property(t => t.cm_texthtml).HasColumnName("cm_texthtml");
        }
    }
}
