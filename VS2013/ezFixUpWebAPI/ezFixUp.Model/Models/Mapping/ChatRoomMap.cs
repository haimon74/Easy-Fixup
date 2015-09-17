using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class ChatRoomMap : EntityTypeConfiguration<ChatRoom>
    {
        public ChatRoomMap()
        {
            // Primary Key
            this.HasKey(t => t.cr_id);

            // Properties
            this.Property(t => t.cr_name)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.cr_topic)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.cr_password)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ChatRooms");
            this.Property(t => t.cr_id).HasColumnName("cr_id");
            this.Property(t => t.cr_name).HasColumnName("cr_name");
            this.Property(t => t.cr_topic).HasColumnName("cr_topic");
            this.Property(t => t.cr_password).HasColumnName("cr_password");
            this.Property(t => t.cr_maxusers).HasColumnName("cr_maxusers");
            this.Property(t => t.cr_visible).HasColumnName("cr_visible");
        }
    }
}
