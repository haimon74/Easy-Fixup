using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class ChatBanMap : EntityTypeConfiguration<ChatBan>
    {
        public ChatBanMap()
        {
            // Primary Key
            this.HasKey(t => new { t.cb_id, t.cb_date });

            // Properties
            this.Property(t => t.cb_id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.cb_ip)
                .HasMaxLength(15);

            // Table & Column Mappings
            this.ToTable("ChatBans");
            this.Property(t => t.cb_id).HasColumnName("cb_id");
            this.Property(t => t.cr_id).HasColumnName("cr_id");
            this.Property(t => t.cu_id).HasColumnName("cu_id");
            this.Property(t => t.cb_ip).HasColumnName("cb_ip");
            this.Property(t => t.cb_date).HasColumnName("cb_date");
            this.Property(t => t.cb_dateexpires).HasColumnName("cb_dateexpires");
        }
    }
}
