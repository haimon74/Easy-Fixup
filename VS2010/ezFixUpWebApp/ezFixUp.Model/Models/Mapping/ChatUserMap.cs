using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class ChatUserMap : EntityTypeConfiguration<ChatUser>
    {
        public ChatUserMap()
        {
            // Primary Key
            this.HasKey(t => t.cu_id);

            // Properties
            this.Property(t => t.cu_username)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.cu_password)
                .IsFixedLength()
                .HasMaxLength(40);

            this.Property(t => t.cu_displayname)
                .HasMaxLength(200);

            this.Property(t => t.cu_ip)
                .HasMaxLength(15);

            this.Property(t => t.cu_lastactiveguid)
                .IsFixedLength()
                .HasMaxLength(36);

            this.Property(t => t.cu_thumbimage)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("ChatUsers");
            this.Property(t => t.cu_id).HasColumnName("cu_id");
            this.Property(t => t.cu_username).HasColumnName("cu_username");
            this.Property(t => t.cu_password).HasColumnName("cu_password");
            this.Property(t => t.cu_displayname).HasColumnName("cu_displayname");
            this.Property(t => t.cu_gender).HasColumnName("cu_gender");
            this.Property(t => t.cu_begintime).HasColumnName("cu_begintime");
            this.Property(t => t.cu_lastactive).HasColumnName("cu_lastactive");
            this.Property(t => t.cu_ip).HasColumnName("cu_ip");
            this.Property(t => t.cu_lastactiveguid).HasColumnName("cu_lastactiveguid");
            this.Property(t => t.cu_thumbimage).HasColumnName("cu_thumbimage");
        }
    }
}
