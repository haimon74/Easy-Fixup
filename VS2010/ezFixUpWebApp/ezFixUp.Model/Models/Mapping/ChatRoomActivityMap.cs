using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class ChatRoomActivityMap : EntityTypeConfiguration<ChatRoomActivity>
    {
        public ChatRoomActivityMap()
        {
            // Primary Key
            this.HasKey(t => new { t.cra_userid, t.cra_roomid });

            // Properties
            this.Property(t => t.cra_userid)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.cra_roomid)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.cra_username)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.cra_displayname)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("ChatRoomActivity");
            this.Property(t => t.cra_userid).HasColumnName("cra_userid");
            this.Property(t => t.cra_roomid).HasColumnName("cra_roomid");
            this.Property(t => t.cra_username).HasColumnName("cra_username");
            this.Property(t => t.cra_displayname).HasColumnName("cra_displayname");
            this.Property(t => t.cra_lastonline).HasColumnName("cra_lastonline");
        }
    }
}
