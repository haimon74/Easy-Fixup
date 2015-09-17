using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class MessagesSandboxMap : EntityTypeConfiguration<MessagesSandbox>
    {
        public MessagesSandboxMap()
        {
            // Primary Key
            this.HasKey(t => new { t.u_username, t.ms_message });

            // Properties
            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.ms_message)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("MessagesSandbox");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.ms_message).HasColumnName("ms_message");
            this.Property(t => t.ms_count).HasColumnName("ms_count");
            this.Property(t => t.ms_datetime).HasColumnName("ms_datetime");

            // Relationships
            this.HasRequired(t => t.User)
                .WithMany(t => t.MessagesSandboxes)
                .HasForeignKey(d => d.u_username).WillCascadeOnDelete(false);

        }
    }
}
