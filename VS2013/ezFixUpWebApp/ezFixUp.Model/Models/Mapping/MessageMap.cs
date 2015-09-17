using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class MessageMap : EntityTypeConfiguration<Message>
    {
        public MessageMap()
        {
            // Primary Key
            this.HasKey(t => t.m_id);

            // Properties
            this.Property(t => t.m_from_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.m_to_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.m_body)
                .IsRequired()
                .HasMaxLength(3500);

            this.Property(t => t.m_subject)
                .HasMaxLength(50);

            this.Property(t => t.m_about_username)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("Messages");
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
            this.Property(t => t.m_type_id).HasColumnName("m_type_id");
            this.Property(t => t.m_subject).HasColumnName("m_subject");
            this.Property(t => t.m_about_username).HasColumnName("m_about_username");

            // Relationships
            this.HasOptional(t => t.MessagesType)
                .WithMany(t => t.Messages)
                .HasForeignKey(d => d.m_type_id).WillCascadeOnDelete(false);
            this.HasRequired(t => t.User)
                .WithMany(t => t.Messages)
                .HasForeignKey(d => d.m_from_username).WillCascadeOnDelete(false);
            this.HasRequired(t => t.User1)
                .WithMany(t => t.Messages1)
                .HasForeignKey(d => d.m_to_username).WillCascadeOnDelete(false);

        }
    }
}
