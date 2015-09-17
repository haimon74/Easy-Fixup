using System.Data.Entity.ModelConfiguration;

namespace ezFixUp.Model.Models.Mapping
{
    public class GroupEventsCommentMap : EntityTypeConfiguration<GroupEventsComment>
    {
        public GroupEventsCommentMap()
        {
            // Primary Key
            this.HasKey(t => t.gec_id);

            // Properties
            this.Property(t => t.u_username)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.gec_comment)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("GroupEventsComments");
            this.Property(t => t.gec_id).HasColumnName("gec_id");
            this.Property(t => t.ge_id).HasColumnName("ge_id");
            this.Property(t => t.u_username).HasColumnName("u_username");
            this.Property(t => t.gec_comment).HasColumnName("gec_comment");
            this.Property(t => t.gec_date).HasColumnName("gec_date");

            // Relationships
            this.HasRequired(t => t.GroupEvent)
                .WithMany(t => t.GroupEventsComments)
                .HasForeignKey(d => d.ge_id).WillCascadeOnDelete(false);
            this.HasRequired(t => t.User)
                .WithMany(t => t.GroupEventsComments)
                .HasForeignKey(d => d.u_username).WillCascadeOnDelete(false);

        }
    }
}
