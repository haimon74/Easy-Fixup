using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class FacebookInvitationMap : EntityTypeConfiguration<FacebookInvitation>
    {
        public FacebookInvitationMap()
        {
            // Primary Key
            this.HasKey(t => t.fi_id);

            // Properties
            this.Property(t => t.fi_request_id)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.fi_sender_id)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.fi_recipient_id)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("FacebookInvitations");
            this.Property(t => t.fi_id).HasColumnName("fi_id");
            this.Property(t => t.fi_request_id).HasColumnName("fi_request_id");
            this.Property(t => t.fi_sender_id).HasColumnName("fi_sender_id");
            this.Property(t => t.fi_recipient_id).HasColumnName("fi_recipient_id");
            this.Property(t => t.fi_created_time).HasColumnName("fi_created_time");
            this.Property(t => t.fi_is_credited).HasColumnName("fi_is_credited");
        }
    }
}
