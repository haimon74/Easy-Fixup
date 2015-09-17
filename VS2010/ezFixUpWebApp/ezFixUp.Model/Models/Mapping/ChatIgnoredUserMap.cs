using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class ChatIgnoredUserMap : EntityTypeConfiguration<ChatIgnoredUser>
    {
        public ChatIgnoredUserMap()
        {
            // Primary Key
            this.HasKey(t => new { t.cu_id, t.ciu_ignoreduserid });

            // Properties
            this.Property(t => t.cu_id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ciu_ignoreduserid)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("ChatIgnoredUsers");
            this.Property(t => t.cu_id).HasColumnName("cu_id");
            this.Property(t => t.ciu_ignoreduserid).HasColumnName("ciu_ignoreduserid");
            this.Property(t => t.ciu_ignoredate).HasColumnName("ciu_ignoredate");

            // Relationships
            this.HasRequired(t => t.ChatUser)
                .WithMany(t => t.ChatIgnoredUsers)
                .HasForeignKey(d => d.cu_id).WillCascadeOnDelete(false);
            this.HasRequired(t => t.ChatUser1)
                .WithMany(t => t.ChatIgnoredUsers1)
                .HasForeignKey(d => d.ciu_ignoreduserid).WillCascadeOnDelete(false);

        }
    }
}
