using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class EventCommentMap : EntityTypeConfiguration<EventComment>
    {
        public EventCommentMap()
        {
            // Primary Key
            this.HasKey(t => t.ec_id);

            // Properties
            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.ec_comment)
                .IsRequired()
                .HasMaxLength(2000);

            // Table & Column Mappings
            this.ToTable("EventComments");
            this.Property(t => t.ec_id).HasColumnName("ec_id");
            this.Property(t => t.e_id).HasColumnName("e_id");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.ec_date).HasColumnName("ec_date");
            this.Property(t => t.ec_comment).HasColumnName("ec_comment");

            // Relationships
            this.HasRequired(t => t.Event)
                .WithMany(t => t.EventComments)
                .HasForeignKey(d => d.e_id).WillCascadeOnDelete(false);
            this.HasRequired(t => t.User)
                .WithMany(t => t.EventComments)
                .HasForeignKey(d => d.u_username).WillCascadeOnDelete(false);

        }
    }
}
