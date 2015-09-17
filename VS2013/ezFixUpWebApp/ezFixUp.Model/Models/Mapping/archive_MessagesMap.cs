using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class archive_MessagesMap : EntityTypeConfiguration<archive_Messages>
    {
        public archive_MessagesMap()
        {
            // Primary Key
            this.HasKey(t => new { t.m_id, t.m_from_username, t.m_from_folder, t.m_to_username, t.m_to_folder, t.m_body, t.m_timestamp, t.m_replied_to, t.m_is_read, t.m_pending_approval, t.m_new });

            // Properties
            this.Property(t => t.m_id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.m_from_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.m_from_folder)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.m_to_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.m_to_folder)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.m_body)
                .IsRequired()
                .HasMaxLength(3500);

            this.Property(t => t.m_replied_to)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("archive_Messages");
            this.Property(t => t.m_id).HasColumnName("m_id");
            this.Property(t => t.m_from_username).HasColumnName("m_from_username");
            this.Property(t => t.m_from_folder).HasColumnName("m_from_folder");
            this.Property(t => t.m_to_username).HasColumnName("m_to_username");
            this.Property(t => t.m_to_folder).HasColumnName("m_to_folder");
            this.Property(t => t.m_body).HasColumnName("m_body");
            this.Property(t => t.m_timestamp).HasColumnName("m_timestamp");
            this.Property(t => t.m_replied_to).HasColumnName("m_replied_to");
            this.Property(t => t.m_is_read).HasColumnName("m_is_read");
            this.Property(t => t.m_pending_approval).HasColumnName("m_pending_approval");
            this.Property(t => t.m_new).HasColumnName("m_new");
        }
    }
}
