using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class ChatRoomAdminMap : EntityTypeConfiguration<ChatRoomAdmin>
    {
        public ChatRoomAdminMap()
        {
            // Primary Key
            this.HasKey(t => new { t.cu_id, t.cr_id });

            // Properties
            this.Property(t => t.cu_id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.cr_id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("ChatRoomAdmins");
            this.Property(t => t.cu_id).HasColumnName("cu_id");
            this.Property(t => t.cr_id).HasColumnName("cr_id");
        }
    }
}
